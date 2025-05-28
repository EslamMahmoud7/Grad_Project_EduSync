using Domain.Interfaces;
using Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services.TokenService
{
    public class GenerateToken : ITokenService
    {
        private readonly IConfiguration _configuration;

        public GenerateToken(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<string> GenerateJWTToken(User user)
        {
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.Role, user.Role.ToString()),
        new Claim("FullName", $"{user.FirstName} {user.LastName}")
    };

            var AuthKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTToken:AuthKey"] ?? "auth key null"));

            var Credentials = new SigningCredentials(AuthKey, SecurityAlgorithms.HmacSha256Signature);

            var Token = new JwtSecurityToken(
                        audience: _configuration["JWTToken:ValidAudience"],
                        issuer: _configuration["JWTToken:ValidIssuer"],
                        expires: DateTime.Now.AddMinutes(double.Parse(_configuration["JWTToken:DurationInMinutes"] ?? "expire is null days")),
                        claims: claims,
                        signingCredentials: Credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(Token);

        }
    }
}
