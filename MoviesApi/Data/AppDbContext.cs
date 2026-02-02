using Microsoft.EntityFrameworkCore;
using MoviesApi.Entities;
using MoviesApi.Enums.Movie;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Movie> Movies { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Movie>()
            .HasQueryFilter(x => x.Status == MovieStatus.Online);
    }
}
