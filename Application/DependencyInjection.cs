using Application.Interfaces.Mappers;
using Application.Interfaces.Services;
using Application.Mapping;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IMovieService, MovieService>();
            services.AddScoped<IMovieMapping, MovieMapping>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserMapping, UserMapping>();

            services.AddScoped<IVoteService, VoteService>();
            services.AddScoped<IVoteMapping, VoteMapping>();

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IJwtTokenService, JwtTokenService>();

            return services;
        }
    }
}
