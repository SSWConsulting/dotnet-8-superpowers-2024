using HeroJobs.Application.Features.HeroJobs.Commands.CreateJob;
using HeroJobs.Application.Features.HeroJobs.Queries.GetAllJobs;
using MediatR;
using HeroJobs.WebApi.Extensions;

namespace HeroJobs.WebApi.Features;

public static class HeroJobEndpoints
{
    public static void MapHeroJobEndpoints(this WebApplication app)
    {
        var group = app
            .MapGroup("HeroJobs")
            .WithTags("HeroJobs")
            .WithOpenApi();

        group
            .MapGet("/", (ISender sender, CancellationToken ct)
                => sender.Send(new GetAllHeroJobsQuery(), ct))
            .WithName("GetHeroJobs")
            .ProducesGet<HeroJobDto[]>();

        // TODO: Investigate examples for swagger docs. i.e. better docs than:
        // myWeirdField: "string" vs myWeirdField: "this-silly-string"
        // (https://github.com/SSWConsulting/HeroJobs/issues/79)

        group
            .MapPost("/", (ISender sender, CreateHeroJobCommand command, CancellationToken ct) => sender.Send(command, ct))
            .WithName("CreateHeroJob")
            .ProducesPost();
    }
}