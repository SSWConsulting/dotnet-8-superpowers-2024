using Spectre.Console;
using Spectre.Console.Cli;

namespace HeroFinder.Console.Heroes.CreateHero;

public class CreateHeroCommand(HeroService heroService) : Command<CreateHeroSettings>
{
    public override int Execute(CommandContext context, CreateHeroSettings settings)
    {
        var hero = new Hero { Name = settings.Name, Alias = settings.Alias, };

        if (settings.Powers?.Any() ?? false)
        {
            hero.HeroPowers = settings.Powers.Select(p => new Power() { Name = p }).ToList();
        }

        heroService.CreateHero(hero);

        AnsiConsole.WriteLine("Hero created: {0} ({1}) - {2}", hero.Name, hero.Alias, hero.GetPowersString());

        return 0;
    }
}