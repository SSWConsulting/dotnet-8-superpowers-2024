using HeroFinder.Console.Common;
using HeroFinder.Console.Heroes;
using HeroFinder.Console.Heroes.CreateHero;
using HeroFinder.Console.Heroes.DispatchHero;
using HeroFinder.Console.Heroes.ListHeroes;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Spectre.Console;
using Spectre.Console.Cli;
using Serilog;

AnsiConsole.Write(new FigletText("Hero Finder").Color(Color.Purple));
AnsiConsole.WriteLine();

// Setup host with logging & configuration

var builder = Host.CreateDefaultBuilder(args);

// Add services to the container
builder.ConfigureServices((ctx, services) =>
{
    services.AddSingleton<HeroService>();
    services.AddSerilog(config => config.ReadFrom.Configuration(ctx.Configuration));
});

var registrar = new TypeRegistrar(builder);
var app = new CommandApp(registrar);

// Register all commands
app.Configure(config =>
{
    // Branches allow us to have subcommands
    config.AddBranch("heroes", heroes =>
    {
        heroes.SetDescription("Management of heroes");

        heroes.AddCommand<ListHeroesCommand>("list")
            .WithDescription("Lists all available heroes.")
            .WithExample("list");

        heroes.AddCommand<CreateHeroCommand>("create")
            .WithDescription("Creates a new hero")
            .WithExample("create", "wolverine", "Logan");
    });

    // Top-level command
    config.AddCommand<DispatchHeroCommand>("sos")
        .WithDescription("Dispatches a free superhero to save the day")
        .WithExample("sos");
});

return app.Run(args);