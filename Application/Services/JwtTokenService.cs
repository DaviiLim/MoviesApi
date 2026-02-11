using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Domain.DTOs.User.Jwt;
using Domain.Interfaces.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Services
{
    public class JwtTokenService : IJwtTokenService
    {

        private readonly IConfiguration _configuration;

        public JwtTokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GenerateToken(UserJwt jwtEntity)
        {

            var claims = new List<Claim>
            {
            new Claim(ClaimTypes.NameIdentifier, jwtEntity.Id.ToString()),
            new Claim(ClaimTypes.Name, jwtEntity.Email),
            new Claim(ClaimTypes.Role,jwtEntity.Role.ToString()),
        };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])         
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expireMinutes = int.Parse(_configuration["Jwt:ExpireMinutes"]);

            var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],                                            
                audience: _configuration["Jwt:Audience"],                                 
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expireMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
