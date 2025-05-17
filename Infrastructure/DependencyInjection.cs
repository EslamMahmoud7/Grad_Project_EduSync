using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Domain.Interfaces;
using Domain.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Infrastructure.Services.TokenService;
using Infrastructure.Services.AccountService;
using Domain.Interfaces.IServices;
using Infrastructure.Services;
using Domain.Interfaces.Repositories;
using Infrastructure.Repositories;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationDI(this IServiceCollection services, IConfiguration configurations)
        {
            services.ServicesRegisteration();
            services.JWTTokenConfiguration(configurations);
            services.DatabaseContexts(configurations);
            services.IdentityConfiguration();
            return services;
        }

        public static IServiceCollection ServicesRegisteration(this IServiceCollection services)
        {
            services.AddScoped<ITokenService, GenerateToken>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<ILectureService, LectureService>();
            services.AddScoped<IGenericRepository<Course>, GenericRepository<Course>>();
            services.AddScoped<IGenericRepository<Lecture>, GenericRepository<Lecture>>();

            services.AddScoped<IPaginationService, PaginationService>();
            return services;
        }
        public static IServiceCollection JWTTokenConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication().AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidAudience = configuration["JWTToken:ValidAudience"],

                    ValidateIssuer = true,
                    ValidIssuer = configuration["JWTToken:ValidIssuer"],

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTToken:AuthKey"] ?? string.Empty)),

                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,

                    RoleClaimType = ClaimTypes.Role,
                };
            });
            return services;
        }

        public static IServiceCollection DatabaseContexts(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<MainDBContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("Infrastructure")));
            services.AddDbContext<IdentityContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("IdentityConnection")));
            return services;
        }
        public static IServiceCollection IdentityConfiguration(this IServiceCollection services)
        {
            services.AddIdentity<Student, IdentityRole>(option =>
            {
                option.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultProvider;
                option.Password.RequiredUniqueChars = 2;
                option.Password.RequireDigit = false;
                option.Password.RequiredLength = 8;
                option.Password.RequireNonAlphanumeric = false;
                option.Password.RequiredUniqueChars = 0;
                option.Password.RequireLowercase = false;
                option.Password.RequireUppercase = false;
            }).AddEntityFrameworkStores<IdentityContext>().AddDefaultTokenProviders();
            return services;
        }
    }
}
