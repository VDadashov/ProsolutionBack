using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProSolution.Core.Entities.Identity;
using ProSolution.Core.Repositories;
using ProSolution.Core.Repositories.Common;
using ProSolution.DAL.Contexts;
using ProSolution.DAL.Repositories;
using ProSolution.DAL.Repositories.Common;
using Pomelo.EntityFrameworkCore.MySql; 

namespace ProSolution.DAL
{
    public static class DALServiceRegistration
    {
        public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDatabase(configuration);
            services.AddIdentity();
            services.AddDALRepositories();
   
            return services;
        }
        public static void AddDALRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<IBadgeRepository, BadgeRepository>();
            services.AddScoped<IBlogRepository, BlogRepository>();
            services.AddScoped<IBrandRepository, BrandRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IOurServiceRepository, OurServiceRepository>();
            services.AddScoped<IPartnerRepository, PartnerRepository>();
         
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ISeoDataMetaRepository, SeoDataMetaRepository>();
            services.AddScoped<ISeoDataUrlRepository, SeoDataUrlRepository>();
            services.AddScoped<ISeoDataRepository, SeoDataRepository>();
            services.AddScoped<ISettingRepository, SettingRepository>();
            services.AddScoped<ISliderReposiroty, SliderRepository>();
            services.AddScoped<IUserAddressRepository, UserAddressRepository>();
            services.AddScoped<IWishlistIemRepostitory,WishListItemRepository>();
            services.AddScoped<IFeatureOptionRepository, FeatureOptionRepository>();
            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddScoped<IProductFeatureKeysRepository, ProductFeatureKeysRepository>();
            







        }
        private static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("MsSql");

            services.AddDbContext<AppDbContext>(options =>
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), mysqlOptions =>
            {
                mysqlOptions.EnableRetryOnFailure();
                }));
        }
    

        private static void AddIdentity(this IServiceCollection services)
        {
           
            services.AddIdentityCore<User>(options => options.SignIn.RequireConfirmedAccount = false) // Use AddIdentityCore instead  
               .AddRoles<IdentityRole>()
               .AddEntityFrameworkStores<AppDbContext>()
               ;
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 1;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                options.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;
            });
        }
    }
    
}

