using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace EntityFrameworkCore.Console;

public class HeroDbContext : DbContext
{
    public DbSet<Hero> Heroes { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseSqlServer(
                @"Server=.\SQL2022;Database=EfCore8Demo;Trusted_Connection=True;TrustServerCertificate=True;");

        optionsBuilder.LogTo(System.Console.WriteLine, new[] { RelationalEventId.CommandExecuted });
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<Hero>();
        entity.ComplexProperty(e => e.SecretHideout);

        //entity.OwnsMany(e => e.HeroPowers, builder => builder.ToJson());
    }
}