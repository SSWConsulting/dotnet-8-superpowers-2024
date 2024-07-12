namespace JusticeLeague.Api.Models;

public record Hero(string HeroName, string CivilianName)
{
    public Guid Id { get; init; } = Guid.NewGuid();
}