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
        _db.Database.EnsureCreated();
        //db.Database.Migrate();

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
}