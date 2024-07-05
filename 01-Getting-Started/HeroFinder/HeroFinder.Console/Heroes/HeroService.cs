namespace HeroFinder.Console.Heroes;

/// <summary>
/// Business Logic for managing heroes
/// </summary>
public class HeroService
{
    private List<Hero> _heroes = HeroFactory.CreateHeroes();

    public IReadOnlyList<Hero> GetHeroes()
    {
        return _heroes.AsReadOnly();
    }

    public Hero? DispatchHero(int heroId)
    {
        var hero = _heroes.FirstOrDefault(h => h.HeroId == heroId);
        if (hero is null)
            return null;

        hero.IsBusy = true;
        return hero;
    }

    public void CreateHero(Hero hero)
    {
        // NOTE: This will not actually be persisted anywhere
        _heroes.Add(hero);
    }
}