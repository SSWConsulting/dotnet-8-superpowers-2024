using Microsoft.Extensions.Logging;

using Spectre.Console;
using Spectre.Console.Cli;

namespace HeroFinder.Console.Heroes.DispatchHero;

public class DispatchHeroCommand(HeroService heroService, ILogger<DispatchHeroCommand> logger) : Command
{
    public override int Execute(CommandContext context)
    {
        logger.LogInformation("A Citizen needs help!");

        // Updates the UI dynamically based on the current task
        AnsiConsole.Status()
            .Start("Searching for superheroes...", ctx =>
            {
                var heroes = heroService.GetHeroes();
                Thread.Sleep(2000);

                foreach (Hero hero in heroes)
                {
                    ctx.Status($"Checking if {hero.Name} is free...");
                    Thread.Sleep(2000);
                    if (hero.IsBusy)
                    {
                        var msg = $"{hero.Name} is not free.";
                        AnsiConsole.WriteLine(msg);
                        logger.LogWarning(msg);
                        continue;
                    }

                    AnsiConsole.WriteLine($"Dispatching {hero.Name} - Hold tight citizen!");
                    heroService.DispatchHero(hero.HeroId);
                    break;
                }
            });

        return 0;
    }
}