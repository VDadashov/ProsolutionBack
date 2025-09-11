using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProSolution.BL.DTOs;
using ProSolution.BL.DTOs.Token;
using ProSolution.BL.DTOs.User;
using ProSolution.BL.Exceptions.Common;
using ProSolution.BL.Services.Interfaces;
using ProSolution.Core.Entities;
using ProSolution.Core.Entities.Commons;
using ProSolution.Core.Entities.Identity;
using ProSolution.Core.Enums;
using ProSolution.Core.Repositories;
using System.Security.Claims;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IEmailService _emailService;
    private readonly IConfiguration _configuration;
    private readonly ITokenHandler _tokenHandler;
    private readonly IMapper _mapper;
    private readonly RoleManager<IdentityRole> _roleManager;
   

    public AuthService(UserManager<User> userManager,
                       SignInManager<User> signInManager,
                       IEmailService emailService,
                       IConfiguration configuration,
                       ITokenHandler tokenHandler,
                       IMapper mapper,
                       RoleManager<IdentityRole> roleManager
                      ) 
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _emailService = emailService;
        _configuration = configuration;
        _tokenHandler = tokenHandler;
        _mapper = mapper;
        _roleManager = roleManager;
      
    }


    public async Task RegisterAsync(RegisterUserDTO dto)
    {
        var existingUser = await _userManager.FindByEmailAsync(dto.Email);
        if (existingUser != null)
        {
            throw new Exception("User with this email already exists.");
        }

        var randomPassword = GenerateSecurePassword();
        var username = dto.Email.Split('@')[0];
        var name = dto.Email.Split('@')[0];
        var surname = dto.Email.Split('@')[0];
        var slug = GenerateSlug(username);

        var newUser = new User
        {
            UserName = username,
            Email = dto.Email,
            Name = name,
            Surname = surname,
            IsActivate = true,
            Slug = slug
        };

        var result = await _userManager.CreateAsync(newUser, randomPassword);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new Exception($"User creation failed: {errors}");
        }

        await _userManager.AddToRoleAsync(newUser, EUserRole.Member.ToString());

        await _emailService.SendPasswordAsync(dto.Email, username, randomPassword);
    }

    // Метод для генерации slug
    private string GenerateSlug(string input)
    {
        var normalized = input.ToLowerInvariant();
        normalized = System.Text.RegularExpressions.Regex.Replace(normalized, @"[^a-z0-9\s-]", "");
        normalized = System.Text.RegularExpressions.Regex.Replace(normalized, @"\s+", "-");
        return normalized.Trim('-');
    }

    public async Task<TokenResponseDto> LoginAsync(LoginUserDTO dto)
    {
       
        User? user = await _userManager.FindByNameAsync(dto.UsernameOrEmail);
        if (user == null)
        {
            user = await _userManager.FindByEmailAsync(dto.UsernameOrEmail);
            if (user == null) throw new UnauthorizedAccessException();
        }

        if (!user.IsActivate)
            throw new UnauthorizedAccessException("User is not active");

        SignInResult result = await _signInManager.PasswordSignInAsync(user, dto.Password, false, true);
        if (result.IsLockedOut)
            throw new UnauthorizedAccessException("The user is blocked and try again after 5 minutes");
        if (!result.Succeeded)
            throw new UnauthorizedAccessException();

        return await _createAccesToken(user);
    }

    public async Task UpdateProfileAsync(string userId, UpdateProfileDTO dto)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) throw new Exception("User not found");

        bool profileDataChanged = false;

        // --- НАДЕЖНОЕ РУЧНОЕ ОБНОВЛЕНИЕ С ВАЛИДАЦИЕЙ ---

        // 1. Обновляем простые поля
        if (!string.IsNullOrWhiteSpace(dto.Name) && dto.Name != "string" && dto.Name != user.Name)
        {
            user.Name = dto.Name;
            profileDataChanged = true;
        }

        if (!string.IsNullOrWhiteSpace(dto.Surname) && dto.Surname != "string" && dto.Surname != user.Surname)
        {
            user.Surname = dto.Surname;
            profileDataChanged = true;
        }

        // 2. Обновляем уникальные поля с дополнительной проверкой
        if (!string.IsNullOrWhiteSpace(dto.Username) && dto.Username != "string" && dto.Username != user.UserName)
        {
            // Проверяем, не занят ли новый Username другим пользователем
            var existingUserByUsername = await _userManager.FindByNameAsync(dto.Username);
            if (existingUserByUsername != null && existingUserByUsername.Id != userId)
            {
                throw new Exception($"Username '{dto.Username}' is already taken.");
            }
            user.UserName = dto.Username;
            profileDataChanged = true;
        }

        if (!string.IsNullOrWhiteSpace(dto.Email) && dto.Email != "string" && dto.Email != user.Email)
        {
            // Проверяем, не занят ли новый Email другим пользователем
            var existingUserByEmail = await _userManager.FindByEmailAsync(dto.Email);
            if (existingUserByEmail != null && existingUserByEmail.Id != userId)
            {
                throw new Exception($"Email '{dto.Email}' is already taken.");
            }
            user.Email = dto.Email;
            profileDataChanged = true;
        }

        // 3. Обновляем слаг, если имя или фамилия были изменены
        if (profileDataChanged)
        {
            var slugSource = $"{user.Name} {user.Surname}";
            var newSlug = GenerateSlug(slugSource); // Предполагается, что у вас есть этот метод
            if (newSlug != user.Slug)
            {
                user.Slug = newSlug;
            }

            // Сохраняем все изменения профиля, если они были
            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                var profileErrors = string.Join(", ", updateResult.Errors.Select(e => e.Description));
                throw new Exception($"Profile update failed: {profileErrors}");
            }
        }

        // 4. Логика смены пароля (выполняется отдельно, так как требует CurrentPassword и сама сохраняет изменения)
        if (!string.IsNullOrWhiteSpace(dto.CurrentPassword) && dto.CurrentPassword != "string" &&
            !string.IsNullOrWhiteSpace(dto.NewPassword) && dto.NewPassword != "string")
        {
            // Если данные профиля менялись, перезагружаем пользователя, чтобы избежать проблем с ConcurrencyStamp
            if (profileDataChanged)
            {
                user = await _userManager.FindByIdAsync(userId);
                if (user == null) throw new Exception("User not found after profile update.");
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                var passwordErrors = string.Join(", ", changePasswordResult.Errors.Select(e => e.Description));
                throw new Exception($"Password change failed: {passwordErrors}");
            }
        }
    }

    public async Task<AppUserGetDto> IsAuthenticatedAsync(ClaimsPrincipal user)
    {
        var isAuthenticated = user?.Identity?.IsAuthenticated ?? false;

        if (!isAuthenticated)
        {
            return new AppUserGetDto
            {
                isAutuhenticated = false
            };
        }

        // Получение данных из Claims
        var id = user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
        var username = user.Identity?.Name ?? "";
        var name = user.FindFirst(ClaimTypes.GivenName)?.Value ?? "";
        var surname = user.FindFirst(ClaimTypes.Surname)?.Value ?? "";
        var email = user.FindFirst(ClaimTypes.Email)?.Value ?? "";
        var role = user.FindFirst(ClaimTypes.Role)?.Value;

        return new AppUserGetDto
        {
            Id = id,
            UserName = username,
            Name = name,
            Surname = surname,
            Email = email,
            Role = role,
            isAutuhenticated = true
        };
    }



    public string? GetUserId(ClaimsPrincipal user)
    {
        return user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }


    private string GenerateSecurePassword()
    {
        var guidPart = Guid.NewGuid().ToString("N").Substring(0, 6);
        return $"A{guidPart}!";
    }
   
    public async Task ForgotPasswordAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
            throw new Exception("User not found");

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        // You might want to encode the token for use in a URL
        var encodedToken = Uri.EscapeDataString(token);

        var resetLink = $"https://prosolution.devhost.site/myaccount/lost-password/{encodedToken}/{email}";
  
        var message = $@"
        <h3>Salam, {user.UserName}!</h3>
        <p>Şifrənizi sıfırlamaq üçün aşağıdakı linkə klikləyin:</p>
        <p><a href='{resetLink}'>Şifrəni sıfırla</a></p>
        <p>Əgər bu istəyi siz etməmisinizsə, bu e-məktubu nəzərə almayın.</p>";

        await _emailService.SendHtmlEmailAsync(user.Email, "🔐 Şifrə sıfırlama istəyi", message);
    }

    public async Task ResetPasswordAsync(ResetPasswordDTO dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null)
            throw new Exception("User not found");

        // 🔐 Проверка формата пароля вручную
        var passwordValidationResult = await _userManager.PasswordValidators[0].ValidateAsync(_userManager, user, dto.NewPassword);
        if (!passwordValidationResult.Succeeded)
        {
            var errors = string.Join(", ", passwordValidationResult.Errors.Select(e => e.Description));
            throw new Exception($"Invalid password format: {errors}");
        }

        var result = await _userManager.ResetPasswordAsync(user, Uri.UnescapeDataString(dto.Token), dto.NewPassword);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new Exception($"Password reset failed: {errors}");
        }
    }

    public async Task<AppUserGetDto> GetUserByIdAsync(string id)
    {
        User? user = await _getByIdAsync(id, false, true);

        return _mapper.Map<AppUserGetDto>(user);
    }

    public async Task<AppUserGetDto> GetUserByUserNameAsync(string userName)
    {
        User? user = await _getByUserNameAsync(userName, false, true);
        if (user is null)
            throw new NotFoundException<User>($"{userName}-User is not found!");

        return _mapper.Map<AppUserGetDto>(user);
    }

    public async Task<ICollection<AppUserGetDto>> GetAllUsersAsync(string? search, bool isActivate)
    {
        ICollection<User> users = await _userManager.Users
            .Where(x => !string.IsNullOrEmpty(search) ? x.UserName.ToLower().Contains(search.ToLower()) : true)
            .Where(x => x.IsActivate == isActivate)
            .ToListAsync();

        return _mapper.Map<ICollection<AppUserGetDto>>(users);
    }

    public async Task<PaginationDto<AppUserGetDto>> GetAllUsersFilteredAsync(string? search, int take, int page, int order, bool isActivate)
    {
        if (page <= 0)
            throw new Exception("Invalid page number.");
        if (take <= 0)
            throw new Exception("Invalid take value.");
        if (order <= 0 || order > 5)
            throw new Exception("Invalid order value.");

        double count = await _userManager.Users.Where(x => !string.IsNullOrEmpty(search) ? x.UserName.ToLower().Contains(search.ToLower()) : true)
                    .Where(x => x.IsActivate == isActivate).CountAsync();

        ICollection<User> users = new List<User>();

        switch (order)
        {
            case 1:
                users = await _userManager.Users.Where(x => !string.IsNullOrEmpty(search) ? x.UserName.ToLower().Contains(search.ToLower()) : true)
                    .Where(x => x.IsActivate == isActivate).OrderBy(x => x.UserName).Skip((page - 1) * take).Take(take)
                    .AsNoTracking().ToListAsync();
                break;
            case 2:
                users = await _userManager.Users.Where(x => !string.IsNullOrEmpty(search) ? x.UserName.ToLower().Contains(search.ToLower()) : true)
                    .Where(x => x.IsActivate == isActivate).OrderByDescending(x => x.UserName).Skip((page - 1) * take).Take(take)
                    .AsNoTracking().ToListAsync();
                break;
            case 3:
                users = await _userManager.Users.Where(x => !string.IsNullOrEmpty(search) ? x.UserName.ToLower().Contains(search.ToLower()) : true)
                    .Where(x => x.IsActivate == isActivate).OrderBy(x => x.Name).Skip((page - 1) * take).Take(take)
                     .AsNoTracking().ToListAsync();
                break;
            case 4:
                users = await _userManager.Users.Where(x => !string.IsNullOrEmpty(search) ? x.UserName.ToLower().Contains(search.ToLower()) : true)
                     .AsNoTracking().ToListAsync();
                break;
            
            case 5:
                users = await _getAuthorsAsync(search, isActivate, take, page);
                break;

            
        }

        ICollection<AppUserGetDto> dtos = _mapper.Map<ICollection<AppUserGetDto>>(users);

        return new()
        {
            Take = take,
            Search = search,
            Order = order,
            CurrentPage = page,
            TotalPage = Math.Ceiling(count / take),
            Items = dtos
        };
    }

    public async Task<string> ChangeUserRoleAsync(ChangeRoleDto dto)
    {
        if (dto.Role == (int)EUserRole.SuperAdmin || !Enum.IsDefined(typeof(EUserRole), dto.Role))
            throw new Exception($"{dto.Role}' is not a valid role value.");

        User user = await _getUserByIdAsync(dto.AppUserId);
        if (user is null)
            throw new Exception($"{dto.AppUserId}-this user is not found");

        IdentityRole? role = await _roleManager.FindByNameAsync(Enum.GetName(typeof(EUserRole), dto.Role)!);
        if (role is null)
            throw new Exception($"{dto.Role}-this role is not found");
        string existRole = await GetUserRoleAsync(dto.AppUserId);

        ICollection<Claim> existClaims = await _userManager.GetClaimsAsync(user);

        IdentityResult result = await _userManager.AddToRoleAsync(user, role.Name!);
        if (!result.Succeeded)
            throw new Exception();
        result = await _userManager.RemoveFromRoleAsync(user, existRole);
        if (!result.Succeeded)
            throw new Exception();

        await _userManager.RemoveClaimsAsync(user, existClaims);

        ICollection<Claim> newClaims = await _userClaims(user);
        await _userManager.AddClaimsAsync(user, newClaims);

        return new($"{user.UserName}-User's role is successfully changed");
    }

    public async Task<string> GetUserRoleAsync(string userId)
    {
        User user = await _getUserByIdAsync(userId);

        ICollection<string> roles = await _userManager.GetRolesAsync(user);

        return roles.FirstOrDefault() ?? "null";
    }

    private async Task<User> _getByIdAsync(string id, bool isTracking = true, bool includes = false)
    {
        if (string.IsNullOrEmpty(id))
            throw new Exception("The provided id is null or empty");

        IQueryable<User> query = _userManager.Users;

       


        if (!isTracking) query = query.AsNoTracking();

        User? user = await query.FirstOrDefaultAsync(x => x.Id == id);
        if (user is null)
            throw new NotFoundException<User>($"User not found({id})!");

        return user;
    }

    private async Task<User> _getUserByIdAsync(string id)
    {
        if (string.IsNullOrEmpty(id))
            throw new Exception("The provided id is null or empty");

        User? user = await _userManager.FindByIdAsync(id);

        if (user is null)
            throw new NotFoundException<User>("This user is not found");

        return user;
    }

    private async Task<User> _getByUserNameAsync(string userName, bool isTracking = true, bool includes = false)
    {
        if (string.IsNullOrEmpty(userName))
            throw new Exception("The provided id is null or UserName");

        IQueryable<User> query = _userManager.Users;

        
        if (!isTracking) query = query.AsNoTracking();

        User? user = await query.FirstOrDefaultAsync(x => x.UserName == userName);
        if (user is null)
            throw new NotFoundException<User>($"User not found({userName})!");

        return user;
    }

  
    private async Task<TokenResponseDto> _createAccesToken(User user)
    {
        ICollection<Claim> claims = await _userClaims(user);
        TokenResponseDto token = _tokenHandler.CreateJwt(user, claims, 60);
        user.RefreshToken = token.RefreshToken;
        user.RefreshTokenExpireAt = token.RefreshTokenExpireAt;
        await _userManager.UpdateAsync(user);

        return token;
    }

    private async Task<ICollection<Claim>> _userClaims(User user)
    {
        ICollection<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.Name),
                new Claim(ClaimTypes.Surname, user.Surname),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

        foreach (var role in await _userManager.GetRolesAsync(user))
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        return claims;
    }
    private async Task<ICollection<User>> _getAuthorsAsync(string? search, bool isActivate, int take, int page)
    {
        var allUsers = await _userManager.Users
            .Where(x => !string.IsNullOrEmpty(search) ? x.UserName.ToLower().Contains(search.ToLower()) : true)
            .Where(x => x.IsActivate == isActivate)
            .AsNoTracking()
            .ToListAsync();

        var authors = new List<User>();

        foreach (var user in allUsers)
        {
            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Contains(EUserRole.Author.ToString()))
            {
                authors.Add(user);
            }
        }

        return authors
            .OrderByDescending(x => x.Name)
            .Skip((page - 1) * take)
            .Take(take)
            .ToList();
    }

 

}
