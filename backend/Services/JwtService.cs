// Services/JwtService.cs
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using backend.EP_R_Daniel_Oliveira_Vargas.Models;

namespace backend.EP_R_Daniel_Oliveira_Vargas.Services
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }

    public class JwtService : IJwtService
    {
        private readonly IConfiguration _cfg;
        public JwtService(IConfiguration cfg) => _cfg = cfg;

        public string GenerateToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role,           user.Role.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti,   Guid.NewGuid().ToString())
            };

            var key   = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_cfg["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var exp   = DateTime.UtcNow.AddMinutes(double.Parse(_cfg["Jwt:ExpireMinutes"]!));

            var token = new JwtSecurityToken(
                issuer:            _cfg["Jwt:Issuer"],
                audience:          _cfg["Jwt:Audience"],
                claims:            claims,
                expires:           exp,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
