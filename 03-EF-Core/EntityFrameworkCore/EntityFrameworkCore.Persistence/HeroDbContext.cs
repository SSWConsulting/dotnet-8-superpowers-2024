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
                "Server=localhost,1433;Database=EfCore8Demo;User ID=sa;Password=31MySqlServer#;Encrypt=False;");

            // optionsBuilder.LogTo(System.Console.WriteLine, new[] { RelationalEventId.CommandExecuted });
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<Hero>();
        entity.HasIndex(e => e.AffiliationId);
    }
}