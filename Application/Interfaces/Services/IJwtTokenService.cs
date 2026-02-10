using MoviesApi.DTOs.User;
using MoviesApi.DTOs.User.Jwt;
using MoviesApi.Entities;

namespace MoviesApi.Interfaces.Services
{
    public interface IJwtTokenService
    {
        public string GenerateToken(UserJwt UserJwt);
    }
}
