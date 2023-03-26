using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Components;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using IdunnoAPI.DAL;
using IdunnoAPI.DAL.Services.Interfaces;
using IdunnoAPI.DAL.Services;
using IdunnoAPI.DAL.Repositories.Interfaces;
using IdunnoAPI.DAL.Repositories;
using IdunnoAPI.Auth.Interfaces;
using IdunnoAPI.Auth;
using Microsoft.AspNetCore.Identity;
using IdunnoAPI.Models;
using IdunnoAPI.Helpers;
using IdunnoAPI.Helpers.Interfaces;

namespace IdunnoAPI.Extensions
{
    public static class ServiceCollectionExtensions
    {
        private static readonly IConfiguration _config;
        public static String policyName = "_myAllowSpecificOrigins";
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: policyName, policy => {
                    policy.WithOrigins("https://localhost:3000");
                    policy.AllowAnyHeader();
                    policy.AllowAnyMethod();
                    policy.AllowCredentials();
                });
            });
        }

        public static void AddServices(this IServiceCollection services) 
        {
            services.AddScoped<IPostsService, PostsService>();
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<IBCryptPasswordHasher, BCryptPasswordHasher>();
            services.AddScoped<IJWToken, JWToken>();
        }

        public static void AddAuth(this IServiceCollection services, ConfigurationManager cfg)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => 
            {
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        if (context.Request.Cookies.ContainsKey(cfg["JWT:StoringCookie"]))
                        {
                            context.Token = context.Request.Cookies[cfg["JWT:StoringCookie"]];
                        }
                        return Task.CompletedTask;
                    }
            };
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(cfg["JWT:Key"]))
                };
            });
        }
    }
}
