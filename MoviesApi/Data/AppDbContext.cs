using Microsoft.EntityFrameworkCore;
using MoviesApi.Entities;
using MoviesApi.Enums.Movie;
using MoviesApi.Enums.User;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Movie> Movies { get; set; }
    public DbSet<Vote> Votes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Movie>()
            .HasQueryFilter(x => x.Status == MovieStatus.Online);

        modelBuilder.Entity<User>()
            .HasQueryFilter(u => u.Status == UserStatus.Ativo);

        modelBuilder.Entity<Vote>(v =>
        {
            v.HasKey(v => v.Id);

            v.HasQueryFilter(v => v.Movie.Status == MovieStatus.Online);
            v.HasQueryFilter(v => v.User.Status == UserStatus.Ativo);
            v.HasQueryFilter(v => v.User.Role == UserRole.Admin);

            v.HasOne(v => v.Movie)
                .WithMany(m => m.Votos)
                .HasForeignKey(v => v.MovieId)
                .OnDelete(DeleteBehavior.Cascade);

            v.HasOne(v => v.User)
                .WithMany(m => m.Votos)
                .HasForeignKey(v => v.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            v.HasIndex(v => new { v.UserId, v.MovieId })
                .IsUnique();
        });
    }
}
