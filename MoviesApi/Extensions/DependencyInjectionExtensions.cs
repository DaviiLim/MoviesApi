using Application.Interfaces.Mappers;
using Application.Interfaces.Services;
using Application.Mapping;
using Application.Services;
using Domain.Interfaces.Repositories;
using Infrastructure.Repositories;

namespace Api.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IMovieService, MovieService>();
            services.AddScoped<IMovieMapping, MovieMapping>();
            services.AddScoped<IMovieRepository, MovieRepository>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserMapping, UserMapping>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IVoteService, VoteService>();
            services.AddScoped<IVoteMapping, VoteMapping>();
            services.AddScoped<IVoteRepository, VoteRepository>();

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IJwtTokenService, JwtTokenService>();

            return services;
        }
    }
}
