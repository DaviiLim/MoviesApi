using Microsoft.IdentityModel.Tokens;
using MoviesApi.DTOs.User;
using MoviesApi.DTOs.User.Jwt;
using MoviesApi.Entities;
using MoviesApi.Interfaces.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime;
using System.Security.Claims;
using System.Text;

namespace MoviesApi.Services
{
    public class JwtTokenService : IJwtTokenService
    {
        //Ajustar para as configurações ( fazer _settings ) 
        public string GenerateToken(UserJwt jwtEntity)
        {

            var claims = new[]
            {
            new Claim("name", jwtEntity.Id.ToString()),
            new Claim("email", jwtEntity.Email),
            new Claim("role",jwtEntity.Role.ToString()),
            new Claim("status",jwtEntity.Status.ToString())
        };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("_settings.Secret_342432432432423")
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
            issuer: "_settings.Issuer",
                audience: "_settings.Audience",
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(-1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
