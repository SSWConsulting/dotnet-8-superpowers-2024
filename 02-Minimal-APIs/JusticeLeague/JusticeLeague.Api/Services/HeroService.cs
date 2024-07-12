using JusticeLeague.Api.Models;

namespace JusticeLeague.Api.Services;

public class HeroService
{
    private readonly List<Hero> _heroNames =
    [
        new Hero("Superman", "Clark Kent"),
        new Hero("Batman", "Bruce Wayne"),
        new Hero("Wonder Woman", "Diana Prince"),
        new Hero("The Flash", "Barry Allen"),
        new Hero("Cyborg", "Victor Stone"),
        new Hero("Aquaman", "Arthur Curry")
    ];

    public IEnumerable<Hero> GetHeroes() => _heroNames;

    public void AddHero(Hero hero) => _heroNames.Add(hero);

    public Hero? GetHeroById(Guid id) => _heroNames.FirstOrDefault(x => x.Id == id);
}