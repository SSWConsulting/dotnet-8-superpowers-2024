using EntityFrameworkCore.Console;

using Microsoft.EntityFrameworkCore;

using Xunit;

namespace EntityFrameworkCore.Test;

public class EfCoreTests
{
    private readonly HeroDbContext _db;

    public EfCoreTests()
    {
        _db = new HeroDbContext();
        _db.Database.EnsureDeleted();
        //_db.Database.EnsureCreated();
        _db.Database.Migrate();

        var data = HeroFactory.CreateHeroes();
        _db.Heroes.AddRange(data);
        _db.SaveChanges();
    }
    
    [Fact]
    public void Query_Heroes()
    {
        var heroes = _db.Heroes
            .Include(i => i.Affiliation)
            .Include(i => i.HeroPowers)
            .ToList();
    }

    [Fact]
    public void Query_With_Unmapped_Types()
    {
        var heroName = "Superman";
        FormattableString sql =
            $"""
             SELECT HeroId, Name, Alias
             FROM Heroes
             WHERE Name = {heroName}
             """;
        var heroNames = _db.Database.SqlQuery<HeroName>(sql).ToList();
    }
    
    [Fact]
    public void Query_Json_Array()
    {
        var superpowers = _db.Heroes
            .AsNoTracking()
            .SelectMany(h => h.HeroPowers.Where(hp => hp.Name.Contains("Super")))
            .ToList();
    }
}