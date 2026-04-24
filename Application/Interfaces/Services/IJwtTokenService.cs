using Domain.DTOs.User;
using Domain.DTOs.User.Jwt;
using Domain.Entities;

namespace Application.Interfaces.Services
{
    public interface IJwtTokenService
    {
        public string GenerateToken(UserJwt UserJwt);
    }
}
