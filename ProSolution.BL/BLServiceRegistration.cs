using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ProSolution.BL.MapperProfiles;
using ProSolution.BL.Mappers;
using ProSolution.BL.MappingProfiles;
using ProSolution.BL.Services.Implements;
using ProSolution.BL.Services.Interfaces;
using ProSolution.BL.Settings;
using ProSolution.BL.Validators.ProductValidator;
using ProSolution.Core.Entities.Identity;
using ProSolution.DAL.Contexts;
using System.Text;
using a = ProSolution.BL.Services.Implements;

namespace ProSolution.BL
{
    public static class BLServiceRegistration
    {
        public static IServiceCollection AddBlServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddServices(configuration);
            //services.RegisterAutoMapper();

            // JWT Authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,

                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecurityKey"])),
                    LifetimeValidator = (notBefore, expires, token, param) => token != null ? expires > DateTime.UtcNow : false
                };
            });

            services.AddIdentity<User, IdentityRole>()
                    .AddEntityFrameworkStores<AppDbContext>()
                    .AddDefaultTokenProviders();

            services.AddAutoMapper(typeof(ProductMP).Assembly);
            services.AddFluentValidation(options => options.RegisterValidatorsFromAssemblyContaining(typeof(ProductCreateDtoValidator)));
            return services;
        }

        private static void AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IBasketService, BasketService>();
            services.AddScoped<IBadgeService, BadgeService>();
            services.AddScoped<IBlogService, BlogService>();
            services.AddScoped<IBrandService, BrandService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IOurServiceService, OurServiceService>();
            services.AddScoped<IPartnerService, PartnerService>();
            services.AddScoped<ISeoDataService, SeoDataService>();
            services.AddScoped<ISeoMetaService, SeoMetaService>();
            services.AddScoped<ISeoUrlService, SeoUrlService>();
            services.AddScoped<ISliderService, SliderService>();
            services.AddScoped<ISettingService, SettingService>();
            services.AddScoped<IUserAddressService, UserAddressService>();
            services.AddScoped<IWishlistService, WishlistService>();
            services.AddScoped<IFeatureOptionService, FeatureOptionService>();
            services.AddScoped<IReviewService, ReviewService>();

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IProductFeatureKeysService, ProductFeatureKeysService>();
            services.AddHostedService<BasketCleanupService>();

            services.AddScoped<ITokenHandler, a.TokenHandler>();

            services.AddScoped<IEmailService, EmailService>();
            services.Configure<SmtpSettings>(configuration.GetSection("SmtpSettings"));

            services.AddScoped<ICloudStorageService, CloudStorageService>();
            services.Configure<CloudinarySettings>(configuration.GetSection("CloudinarySettings"));
        }

        private static void RegisterAutoMapper(this IServiceCollection services)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ProductMP());
                cfg.AddProfile(new SEOMP());
                cfg.AddProfile(new SEOMetaMP());
                cfg.AddProfile(new SEOUrlMP());
                cfg.AddProfile(new SliderMP());
                cfg.AddProfile(new BadgeMP());
                cfg.AddProfile(new BlogMP());
                cfg.AddProfile(new BrandMP());
                cfg.AddProfile(new CategoryMP());
                cfg.AddProfile(new OurServiceMP());
                cfg.AddProfile(new PartnerMP());
                cfg.AddProfile(new SettingMP());
                cfg.AddProfile(new UserAddressMP());
                cfg.AddProfile(new WishListMP());
            });

            IMapper mapper = config.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}
