using Microsoft.AspNetCore.Identity;

namespace ProSolution.Core.Entities.Identity
{
    public class User : IdentityUser
    {
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;

        public string? Slug { get; set; }
        public bool IsActivate { get; set; } = true;
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpireAt { get; set; }

        public string? UserAddressId { get; set; }
        public UserAddress? UserAddress { get; set; }
        public ICollection<WishlistItem>? WishlistItems { get; set; }
        public ICollection<Blog>? Blogs { get; set; }

    }
}
