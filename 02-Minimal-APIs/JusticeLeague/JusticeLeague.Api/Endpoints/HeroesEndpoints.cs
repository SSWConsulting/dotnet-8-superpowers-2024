using JusticeLeague.Api.Application;

using Microsoft.AspNetCore.Http.HttpResults;

namespace JusticeLeague.Api.Endpoints;

public static class HeroesEndpoints
{
    public static void AddHeroesEndpoints(this WebApplication webApplication)
    {
        var group = webApplication.MapGroup("api/heroes")
            .WithTags("Heroes")
            .RequireAuthorization();

        group
            .MapGet("", (HeroService heroService) => heroService.GetHeroes())
            .WithName("GetHeroes");

        group
            .MapPost("", (string heroName, string civilianName, HeroService heroService) =>
            {
                var hero = new Hero(heroName, civilianName);
                heroService.AddHero(hero);
                return TypedResults.Created($"api/heroes/{hero.Id}", hero);
            })
            .WithName("CreateHeroes");

        group
            .MapGet("{id:Guid}", Results<Ok<Hero>, NotFound> (Guid id, HeroService heroService) =>
            {
                var hero = heroService.GetHeroById(id);
                if (hero == null)
                    return TypedResults.NotFound();

                return TypedResults.Ok(hero);
            })
            .WithName("GetHeroById");
    }
}