using EntityFrameworkCore.Console;

using Microsoft.EntityFrameworkCore;

Console.WriteLine("EF Core 8 Demo");

var db = new HeroDbContext();
// db.Database.EnsureDeleted();
// db.Database.EnsureCreated();
//db.Database.Migrate();

var data = HeroFactory.CreateHeroes();
db.Heroes.AddRange(data);
db.SaveChanges();

// TODO: Update these to be tests
//Query1();
//Query2();
//Update1();
//Query4();
Query5();

Console.Read();

void Query1()
{
    var heroes = db.Heroes
        .Include(i => i.Affiliation)
        .Include(i => i.HeroPowers)
        .ToList();

    foreach (Hero hero in heroes)
    {
        Console.WriteLine(hero);
    }
}

void Query2()
{
    var heroName = "Superman";
    FormattableString sql =
        $"""
         SELECT HeroId, Name, Alias
         FROM Heroes
         WHERE Name = {heroName}
         """;
    var heroNames = db.Database.SqlQuery<HeroName>(sql).ToList();
    foreach (HeroName hero in heroNames)
    {
        Console.WriteLine($"{hero.HeroId} {hero.Name} {hero.Alias}");
    }
}

void Update1()
{
    db.Heroes.ExecuteUpdate(s => s.SetProperty(h => h.SecretHideout.City, "Metropolis"));
}

void Query4()
{
    // TODO: Why doesn't this work?
    var heroes = db.Heroes.Where(h => h.SavedTheCityDates.Contains(new DateOnly(2000, 01, 01))).ToList();
}

void Query5()
{
    var superpowers = db.Heroes
        .AsNoTracking()
        .SelectMany(h => h.HeroPowers.Where(hp => hp.Name.Contains("Super")))
        .ToList();
}