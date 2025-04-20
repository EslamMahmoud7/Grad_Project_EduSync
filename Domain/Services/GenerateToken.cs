using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public class GenerateToken : ITokenService
    {
        private readonly IConfiguration _configuration;

        public GenerateToken(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<string> GenerateJWTToken(Student student)
        {
            var PrivateClaims = new List<Claim>()
            {
                new Claim (ClaimTypes.Name, student.FirstName),
                new Claim (ClaimTypes.Email, student.Email),
                new Claim ("Id", student.Id.ToString()),
            };

            var AuthKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTToken:AuthKey"] ?? "auth key null"));

            var Credentials = new SigningCredentials(AuthKey, SecurityAlgorithms.HmacSha256Signature);

            var Token = new JwtSecurityToken(
                        audience: _configuration["JWTToken:ValidAudience"],
                        issuer: _configuration["JWTToken:ValidIssuer"],
                        expires: DateTime.Now.AddDays(double.Parse(_configuration["JWTToken:DurationInDays"] ?? "expire is null days")),
                        claims: PrivateClaims,
                        signingCredentials: Credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(Token);
        }
    }
}
