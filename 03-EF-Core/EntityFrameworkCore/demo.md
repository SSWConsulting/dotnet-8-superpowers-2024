# Entity Framework Core 8

## Setup

```bash
mkdir EntityFrameworkCore
cd EntityFrameworkCore
mkdir EntityFrameworkCore.Persistence
cd EntityFrameworkCore.Persistence
dotnet new classlib
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
cd ..
mkdir EntityFrameworkCore.Tests
cd EntityFrameworkCore.Tests
dotnet new xunit
cd ..
dotnet new sln
dotnet sln add .\EntityFrameworkCore.Persistence.csproj
dotnet sln add .\EntityFrameworkCore.Tests.csproj
```

Create the persistence project in a file called `Models\Models.cs`

```csharp
public class Hero
{
    public int HeroId { get; set; }
    public string Name { get; set; }
    public string Alias { get; set; }

    public int? AffiliationId { get; set; }
    public Affiliation Affiliation { get; set; }

    public List<Power> HeroPowers { get; set; }
}

public class Power
{
    public int PowerId { get; set; }
    public string Name { get; set; }
}

public class Affiliation
{
    public int AffiliationId { get; set; }
    public string Name { get; set; }
}
```

Create the DbContext:

```csharp
public class HeroDbContext : DbContext
{
    public DbSet<Hero> Heroes { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseSqlServer(
                @"Server=.\SQL2022;Database=EfCore8Demo;Trusted_Connection=True;TrustServerCertificate=True;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}
```

Create a data factory called `HeroFactory`:

```csharp
public static class HeroFactory
{
    public static List<Hero> CreateHeroes()
    {
        var heroes = new List<Hero>
        {
            new()
            {
                Name = "Superman",
                Alias = "Clark Kent",
                Affiliation = AffiliationFactory.JusticeLeague,
                HeroPowers =
                [
                    PowersFactory.Flight,
                    PowersFactory.SuperStrength,
                    PowersFactory.HeatVision,
                    PowersFactory.XRayVision,
                    PowersFactory.SuperSpeed,
                    PowersFactory.SuperHearing,
                    PowersFactory.Invulnerability
                ]
            },
            new()
            {
                Name = "Batman",
                Alias = "Bruce Wayne",
                Affiliation = AffiliationFactory.JusticeLeague,
                HeroPowers =
                [
                    PowersFactory.Wealth,
                    PowersFactory.MartialArts,
                    PowersFactory.Intelligence,
                    PowersFactory.Gadgets
                ]
            },
            new()
            {
                Name = "Wonderwoman",
                Alias = "Diana Prince",
                Affiliation = AffiliationFactory.JusticeLeague,
                HeroPowers = new List<Power>
                {
                    PowersFactory.Flight,
                    PowersFactory.SuperStrength,
                    PowersFactory.SuperSpeed,
                    PowersFactory.Invulnerability

                }
            },
            new()
            {
                Name = "Wolverine",
                Alias = "Logan",
                Affiliation = AffiliationFactory.XMen,
                HeroPowers = new List<Power>()
                {
                    PowersFactory.Regeneration,
                    PowersFactory.AdamantiumClaws,
                    PowersFactory.SuperStrength,
                    PowersFactory.SuperSpeed,
                    PowersFactory.SuperSenses
                }
            },
            new()
            {
                Name = "Cyclops",
                Alias = "Scott Summers",
                Affiliation = AffiliationFactory.XMen,
                HeroPowers =
                [
                    PowersFactory.OpticBlast,
                    PowersFactory.SuperStrength,
                    PowersFactory.SuperSpeed,
                    PowersFactory.SuperSenses
                ]
            },
            // create me a spiderman hero in the same format as above
            new()
            {
                Name = "Spiderman",
                Alias = "Peter Parker",
                Affiliation = AffiliationFactory.XMen,
                HeroPowers =
                [
                    PowersFactory.SuperStrength,
                    PowersFactory.SuperSpeed,
                    PowersFactory.SuperSenses,
                    PowersFactory.Regeneration
                ]
            }

        };

        return heroes;
    }

    private static class AffiliationFactory
    {
        public static Affiliation JusticeLeague { get; } = new() { Name = "Justice League" };
        public static Affiliation XMen { get; } = new() { Name = "X-Men" };
    }

    private static class PowersFactory
    {
        public static Power Flight { get; } = new() { Name = "Flight" };
        public static Power SuperStrength { get; } = new() { Name = "Super Strength" };
        public static Power HeatVision { get; } = new() { Name = "Heat Vision" };
        public static Power XRayVision { get; } = new() { Name = "X-Ray Vision" };
        public static Power SuperSpeed { get; } = new() { Name = "Super Speed" };
        public static Power SuperHearing { get; } = new() { Name = "Super Hearing" };
        public static Power Invulnerability { get; } = new() { Name = "Invulnerability" };
        public static Power Wealth { get; } = new() { Name = "Wealth" };
        public static Power MartialArts { get; } = new() { Name = "Martial Arts" };
        public static Power Intelligence { get; } = new() { Name = "Intelligence" };
        public static Power Gadgets { get; } = new() { Name = "Gadgets" };
        public static Power Regeneration { get; } = new() { Name = "Regeneration" };
        public static Power AdamantiumClaws { get; } = new() { Name = "Adamantium Claws" };
        public static Power SuperSenses { get; } = new() { Name = "Super Senses" };
        public static Power OpticBlast { get; } = new() { Name = "Optic Blast" };
    }
}
```

Rename `UnitTest1` to `EfCoreTests`


Update `EfCoreTests` to create and seed the database:

```csharp
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
```

Look at the database and confirm that the data has been created. Notice how the `HeroPower` many-to-many joining table
was automatically created.

## Migrations

Now we've got a database that can be created and populated with data. But what if we want to change the schema after
it's been deployed to production. For that, we'll need to use migrations.

In the solution root:

```bash
dotnet new tool-manifest
dotnet tool install dotnet-ef
```

Update `EfCoreTests` to use migrations:

```csharp
//db.Database.EnsureCreated();
db.Database.Migrate();
```

Enable migrations via rider to the persistence project.

Add a new property to the `Hero` class:

```csharp
public DateOnly? LastSavedTheCity { get; set; }
```

> **NOTE**: Notice how we are using 'DateOnly' this is new to .NET 8

Add a new migration via Rider

Run the project and ensure the new column is added.

## Bundles

Now, running migrations during start-up of a program is not ideal. In a web farm scenario this can cause issues with
scaling out. Ideally, we want to run our migrations once during deployment and not during start-up. 

In older versions of EF we could script out the migrations and run them manually.

But in EF Core 8 we can bundle the migrations into a single file and run them all at once.

Let's now add a migration bundle.

First, delete the database via Rider UI.

Generate the bundle

```bash
dotnet ef migrations bundle --self-contained --force
```

Execute the migration bundle

```bash
# NOTE: if an appsettings.json can be found the connection string can be automatically picked up from there
.\efbundle.exe --connection "Server=.\SQL2022;Database=EfCore8Demo;Trusted_Connection=True;TrustServerCertificate=True;"
```

Inspect the DB. You'll notice how the tables are now empty. That's because only the migrations are run, and we don't
specifically have any seed data in this case.

## Complex Types

When modeling our entities in EF, we don't have to use flat structures. We can use nested data structures to group
similar properties together and help us reason about our data.

In EF Core 7, we used Owned Entities for this. These mostly worked, but had some limitations as they were using
entities under the hood.

In EF Core 8, we now have Complex Types, which serve a similar purpose, but are not entities under the hood. They more
closely match a typical 'Value Object', that you may see in the DDD world.

Let's add a Complex Type to our model.

First, let's revert back to dropping and creating the DB so we don't have to keep adding migrations.

```csharp
db.Database.EnsureDeleted();
db.Database.EnsureCreated();
//db.Database.Migrate();
```

Add a new class called `SecretHideout` to `Models.cs`

```csharp
public class SecretHideout
{
    public string? Street { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
}
```

Add this to our `Hero` model

```csharp
public SecretHideout SecretHideout { get; set; } = new ();
```

Configure our `HeroDbContext`

```csharp
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // 👇
        var entity = modelBuilder.Entity<Hero>();
        entity.ComplexProperty(e => e.SecretHideout);
        // 👆
    }
```

Add a Hideout for Batman

```csharp
SecretHideout = new SecretHideout
{
    Street = "Bat Cave",
    City = "Gotham",
    Country = "USA"
}
```

Run the solution and check the DB. Notice how the `SecretHideout` is now a nested structure in the `Heroes` table.

## Unmapped queries

Sometimes you might want to run queries against a DB you don't control, and you don't want the extra hassle of setting up an ORM. In those cases you can use EF Core to run unmapped queries.

You may also want to run some SQL that is not supported by EF Core (such as common table expressions, and window functions).

Create a new class called `HeroName` in our test project

```csharp
public class HeroName
{
    public int HeroId { get; set; }
    public string Name { get; set; }
    public string Alias { get; set; }
}
```

Add the following query to `EfCoreTests`

```csharp
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
```

Run and test

## Bulk Updates & Deletes

Let's say you wanted to run a bulk update on a table.  In EF Core 7, we could do that but were limited to a single entity.  However, now in EF Core 8, we can do updates across multiple structures (however, they still need to live in the same table).

Add the following code

```csharp
[Fact]
public void Bulk_Update_With_Nested_Types()
{
    _db.Heroes.ExecuteUpdate(s => s.SetProperty(h => h.SecretHideout.City, "Metropolis"));
}
```

run and test

## Primitive collections

Now as our heroes are getting more active, we want to store the date every time they save the city (instead of just the last date).  One option is to create another table to store this data, but that's a lot of work.  Instead, we can use a primitive collection.

Change the `Hero` class to the following

```csharp
    public List<DateOnly>? SavedTheCityDates { get; set; }
```

Add some dates to one of the heroes

```csharp
SavedTheCityDates =  [
    new DateOnly(2000, 01, 01),
    new DateOnly(2000, 02, 02),
    new DateOnly(2000, 03, 03),
]
```

Add the following query

```csharp
[Fact]
public void Query_Primitive_Collection()
{
    var heroes = _db.Heroes.Where(h => h.SavedTheCityDates.Contains(new DateOnly(2000, 01, 01))).ToList();
}
```

Run the solution and check the database, and inspect the query


## JSON column enhancements

Support for JSON columns was introduced in EF Core 7.  You could query and update JSON columns.  But there were some limitations.

EF Core 8 adds some more advanced JSON capabilities.  Let's take a look.

Add the following configuration to `HeroDbContext`

```csharp
entity.OwnsMany(e => e.HeroPowers, builder => builder.ToJson());
```

This will store our HeroPowers as a JSON document instead of a in a separate table

Add this query to `Program`

```csharp
[Fact]
public void Query_Json_Array()
{
    var superpowers = _db.Heroes
        .AsNoTracking()
        .SelectMany(h => h.HeroPowers.Where(hp => hp.Name.Contains("Super")))
        .ToList();
}
```

Run and inspect the DB schema and data.  Run the query and inspect the JSON.
