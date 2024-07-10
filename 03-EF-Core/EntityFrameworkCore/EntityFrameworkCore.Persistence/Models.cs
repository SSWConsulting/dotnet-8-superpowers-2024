namespace EntityFrameworkCore.Console;

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