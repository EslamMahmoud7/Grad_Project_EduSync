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
            services.AddScoped<IGenericRepository<Course>, GenericRepository<Course>>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<IPaginationService, PaginationService>();
            services.AddScoped<IGenericRepository<Assignment>, GenericRepository<Assignment>>();
            services.AddScoped<IGenericRepository<Announcement>, GenericRepository<Announcement>>();
            services.AddScoped<IAssignmentService, AssignmentService>();
            services.AddScoped<IAnnouncementService, AnnouncementService>();
            services.AddScoped<IDashboardService, DashboardService>();
            services.AddScoped<IGenericRepository<User>, GenericRepository<User>>();
            services.AddScoped<ICourseScheduleService, CourseScheduleService>();
            services.AddScoped<IGenericRepository<GroupStudent>, GenericRepository<GroupStudent>>();
            services.AddScoped<IGenericRepository<Group>, GenericRepository<Group>>();
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<IInstructorService, InstructorService>();
            services.AddScoped<IGenericRepository<AcademicRecord>, GenericRepository<AcademicRecord>>();
            services.AddScoped<IAcademicRecordService, AcademicRecordService>();
            services.AddScoped<IQuizService, QuizService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAdminDashboardService, AdminDashboardService>();
            services.AddScoped<IInstructorDashboardService, InstructorDashboardService>();
            services.AddScoped<IMaterialService, MaterialService>();
            services.AddScoped<ISubmittedAssignmentService, SubmittedAssignmentService>();
            services.AddScoped<IGenericRepository<SubmittedAssignment>, GenericRepository<SubmittedAssignment>>();


            return services;
        }
        public static IServiceCollection JWTTokenConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = configuration["JWTToken:ValidIssuer"],
                        ValidateAudience = true,
                        ValidAudience = configuration["JWTToken:ValidAudience"],
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(configuration["JWTToken:AuthKey"]!)
                        ),
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        NameClaimType = ClaimTypes.NameIdentifier,
                        RoleClaimType = ClaimTypes.Role,
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            Console.WriteLine($"Authentication failed: {context.Exception.Message}");
                            return Task.CompletedTask;
                        },
                        OnTokenValidated = context =>
                        {
                            Console.WriteLine($"Token validated for: {context.Principal?.Identity?.Name}");
                            return Task.CompletedTask;
                        },
                        OnChallenge = context =>
                        {
                            Console.WriteLine($"Challenge triggered: {context.Error}, {context.ErrorDescription}");
                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddAuthorization();
            return services;
        }


        public static IServiceCollection DatabaseContexts(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<MainDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly("Infrastructure")));
            return services;
        }
        public static IServiceCollection IdentityConfiguration(this IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole>(option =>
            {
                option.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultProvider;
                option.Password.RequiredUniqueChars = 2;
                option.Password.RequireDigit = false;
                option.Password.RequiredLength = 8;
                option.Password.RequireNonAlphanumeric = false;
                option.Password.RequiredUniqueChars = 0;
                option.Password.RequireLowercase = false;
                option.Password.RequireUppercase = false;
            }).AddEntityFrameworkStores<MainDbContext>().AddDefaultTokenProviders();
            return services;
        }
    }
}
