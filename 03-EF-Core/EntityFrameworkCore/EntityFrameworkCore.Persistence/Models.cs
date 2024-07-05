namespace EntityFrameworkCore.Console;



// create a class for a hero's database

public class Hero
{
    public int HeroId { get; set; }
    public string Name { get; set; }
    public string Alias { get; set; }

    public int? AffiliationId { get; set; }
    public Affiliation Affiliation { get; set; }

    public List<DateOnly>? SavedTheCityDates { get; set; }

    public SecretHideout SecretHideout { get; set; } = new ();

    public List<Power> HeroPowers { get; set; }

    public override string ToString()
    {
        // return a string that displays all properties of the hero (and include nested properties)
        return $"{HeroId} {Name} {Alias} {Affiliation.Name} {string.Join(", ", HeroPowers.Select(hp => hp.Name))}";
    }
}

public class SecretHideout
{
    public string? Street { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
}

public class Power
{
    public int PowerId { get; set; }
    public string Name { get; set; }

    public List<Hero> Heroes { get; set; }
}


public class Affiliation
{
    public int AffiliationId { get; set; }
    public string Name { get; set; }
}

// Unmapped
public class HeroName
{
    public int HeroId { get; set; }
    public string Name { get; set; }
    public string Alias { get; set; }
}