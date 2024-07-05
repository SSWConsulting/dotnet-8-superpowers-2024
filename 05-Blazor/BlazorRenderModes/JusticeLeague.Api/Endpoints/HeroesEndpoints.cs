using JusticeLeague.Api.Application;

namespace JusticeLeague.Api.Endpoints;

public static class HeroesEndpoints
{
    public static void AddHeroesEndpoints(this WebApplication webApplication)
    {
        var group = webApplication.MapGroup("heroes")
            .WithTags("Heroes")
            .WithOpenApi();

        group
            .MapGet("/", (HeroService heroService) => heroService.GetHeroes())
            .WithName("GetHeroes");

        group
            .MapPost("/", (string heroName, string civilianName, Superpower superpower , HeroService heroService) =>
            {
                var hero = new Hero(heroName, civilianName);
                heroService.AddHero(hero);
                return Results.Created($"/heroes/{hero.Id}", hero);
            })
            .WithName("CreateHero")
            .Produces<Hero>(StatusCodes.Status201Created);

        group
            .MapGet("/{id:Guid}", (Guid id, HeroService heroService) =>
            {
                var hero = heroService.GetHeroById(id);
                if (hero == null)
                    Results.NotFound();

                return hero;
            })
            .WithName("GetHeroById")
            .Produces(StatusCodes.Status404NotFound);
    }
}