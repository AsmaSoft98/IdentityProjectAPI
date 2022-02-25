using identityProjectAPI.Data;
using identityProjectAPI.Infrastructure;
using identityProjectAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;

namespace identityProjectAPI.Extensions
{
    public static class ServiceExtensions
    {
      public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", p => { p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); });
            });
        }
       public static void AddApplicationDatabaseContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        }
        public static void AddIdentityAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            // Add Identity
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                // set password options here
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 4;

            }).AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders(); // adds token for resetting password etc
            // Add Jwt Auth
            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // set up Bearer
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                // validate token
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    ValidAudience = configuration["JwtSettings:Audience"],
                    ValidIssuer = configuration["JwtSettings:Issuer"],
                    RequireExpirationTime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]))
                    
                };
            });
           
        }
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();

        }
     
         public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(sp => new AppConfiguration
            {
                Audience = configuration["JwtSettings:Audience"],
                Issuer = configuration["JwtSettings:Issuer"],
                Key = configuration["JwtSettings:Key"],
                ExpiryInDays = Convert.ToInt32(configuration["JwtSettings:ExpiryInDays"])
            });

            
        }
    }
}
