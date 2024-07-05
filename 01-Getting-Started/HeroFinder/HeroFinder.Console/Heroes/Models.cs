namespace HeroFinder.Console.Heroes;

public class Hero
{
    public int HeroId { get; set; }
    public required string Name { get; init; }
    public required string Alias { get; init; }

    public int? AffiliationId { get; set; }
    public Affiliation? Affiliation { get; set; }

    public bool IsBusy { get; set; } = false;

    public List<DateOnly>? SavedTheCityDates { get; set; }

    public SecretHideout SecretHideout { get; set; } = new ();

    public List<Power>? HeroPowers { get; set; }

    public string GetPowersString()
    {
        var powers = HeroPowers?.Select(hp => hp.Name);
        return powers is null ? string.Empty : string.Join(',', powers);
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
    public required string Name { get; init; }

    public List<Hero>? Heroes { get; set; }
}


public class Affiliation
{
    public int AffiliationId { get; set; }
    public required string Name { get; init; }
}