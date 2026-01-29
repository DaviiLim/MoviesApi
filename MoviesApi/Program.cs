using Microsoft.EntityFrameworkCore;
using MoviesApi.Controllers;
using MoviesApi.Entities;
using MoviesApi.Interfaces.Repositories;
using MoviesApi.Interfaces.Services;
using MoviesApi.Mapping;
using MoviesApi.Repositories;
using MoviesApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("MySqlConnection"),
        ServerVersion.AutoDetect(
            builder.Configuration.GetConnectionString("MySqlConnection")
        )
    )
);

//builder.Services.AddScoped<MovieService>();
//builder.Services.AddScoped<MovieMapping>();
//builder.Services.AddScoped<MovieRepository>();

builder.Services.AddScoped<IUserService,UserService>();
builder.Services.AddScoped<UserMapping>();
builder.Services.AddScoped<IUserRepository,UserRepository>();

builder.Services.AddScoped<AuthController>();
builder.Services.AddScoped<IAuthService,AuthService>();

builder.Services.AddScoped<IJwtTokenService,JwtTokenService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
