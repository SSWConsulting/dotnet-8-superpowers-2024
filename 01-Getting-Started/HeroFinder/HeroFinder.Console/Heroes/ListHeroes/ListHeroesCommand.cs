using Spectre.Console;
using Spectre.Console.Cli;

namespace HeroFinder.Console.Heroes.ListHeroes;

public class ListHeroesCommand(HeroService heroService) : Command
{
    public override int Execute(CommandContext context)
    {
        var heroes = heroService.GetHeroes();

        var table = new Table();

        table.AddColumn("Hero Name");
        table.AddColumn("Hero Alias");
        table.AddColumn("Powers");

        foreach (var hero in heroes)
        {
            table.AddRow(
                hero.Name,
                hero.Alias,
                hero.GetPowersString());
        }

        table.Expand();

        AnsiConsole.Write(table);

        return 0;
    }
}