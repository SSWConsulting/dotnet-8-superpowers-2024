namespace JusticeLeague.Api.Application;

public class Hero(string heroName, string civillianName)
{
    public Guid Id { get; } = Guid.NewGuid();

    public string HeroName { get; } = heroName;

    public string CivillianName { get; } = civillianName;

    public Superpower Superpower { get; } = Superpower.Flight;
}

public enum Superpower
{
    SuperStrength = 1,
    Flight = 2,
    SuperSpeed = 3,
}