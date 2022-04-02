using Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.Data;

public class FelchenContext : DbContext
{
    public FelchenContext(DbContextOptions<FelchenContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<FishCatch> FischCatches { get; set; }
    public DbSet<Log> Logs { get; set; }
    public DbSet<LogType> LogTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>().Property(u => u.FirstName).HasMaxLength(100).IsRequired();
        modelBuilder.Entity<User>().Property(u => u.LastName).HasMaxLength(100).IsRequired();
        modelBuilder.Entity<User>().Property(u => u.Email).HasMaxLength(100).IsRequired();
        modelBuilder.Entity<User>().Property(u => u.Password).IsRequired();
        modelBuilder.Entity<User>().Property(u => u.Salt).IsRequired();
        modelBuilder.Entity<User>().Property(u => u.CreatedAt).IsRequired();

        modelBuilder.Entity<FishCatch>().Property(c => c.NymphName).HasMaxLength(150);
        modelBuilder.Entity<FishCatch>().Property(c => c.NymphColor).HasMaxLength(150);
        modelBuilder.Entity<FishCatch>().Property(c => c.NymphHead).HasMaxLength(100);
        modelBuilder.Entity<FishCatch>().Property(c => c.LakeName).HasMaxLength(150);
        modelBuilder.Entity<FishCatch>().Property(c => c.Weather).HasMaxLength(200);

        modelBuilder.Entity<FishCatch>()
            .HasOne(c => c.User)
            .WithMany()
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Log>().Property(l => l.CratedAt).IsRequired();
        modelBuilder.Entity<Log>().Property(l => l.Message).HasMaxLength(255).IsRequired();
        modelBuilder.Entity<Log>()
            .HasOne(l => l.LogType)
            .WithMany()
            .HasForeignKey(l => l.LogTypeId);

        modelBuilder.Entity<LogType>().Property(lt => lt.Name).IsRequired().HasMaxLength(200);
    }
}