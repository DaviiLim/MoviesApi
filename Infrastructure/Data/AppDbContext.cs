using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Enums.Movie;
using Domain.Enums.User;
using Domain.Enums.Vote;

namespace Infrastructure.Data;
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
            .HasQueryFilter(u => u.Status == UserStatus.Active);

        modelBuilder.Entity<Vote>(v =>
        {
            v.HasKey(v => v.Id);

            v.HasQueryFilter(v => v.Movie.Status == MovieStatus.Online && v.User.Status == UserStatus.Active && v.Status == VoteStatus.Active);

            v.HasOne(v => v.Movie)
                .WithMany(m => m.Votes)
                .HasForeignKey(v => v.MovieId)
                .OnDelete(DeleteBehavior.Restrict);

            v.HasOne(v => v.User)
                .WithMany(u => u.Votes)
                .HasForeignKey(v => v.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            v.HasIndex(v => new { v.UserId, v.MovieId })
                .IsUnique();
        });
    }
}
