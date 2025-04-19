namespace Grad_Project_LMS.Services.TokenService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

public static class TokenConfiguration
{
    public static IServiceCollection ConfigurJWTToken(IServiceCollection services, IConfiguration configuration)
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
}
  

