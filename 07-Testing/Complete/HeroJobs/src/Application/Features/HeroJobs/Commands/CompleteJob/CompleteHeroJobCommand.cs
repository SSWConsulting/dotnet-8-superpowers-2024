using HeroJobs.Application.Common.Exceptions;
using HeroJobs.Application.Common.Interfaces;
using HeroJobs.Domain.HeroJobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HeroJobs.Application.Features.HeroJobs.Commands.CompleteJob;

public record CompleteHeroJobCommand(Guid HeroJobId) : IRequest;

// ReSharper disable once UnusedType.Global
public class CompleteHeroJobCommandHandler(IApplicationDbContext dbContext, ILogger<CompleteHeroJobCommandHandler> logger) : IRequestHandler<CompleteHeroJobCommand>
{
    public async Task Handle(CompleteHeroJobCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("completing job {HeroJobId}", request.HeroJobId);

        var heroJobId = new HeroJobId(request.HeroJobId);

        var heroJob = await dbContext.HeroJobs
            .WithSpecification(new HeroJobByIdSpec(heroJobId))
            .FirstOrDefaultAsync(cancellationToken);

        if (heroJob is null)
            throw new NotFoundException(nameof(heroJob), heroJobId);

        if (heroJob.Done)
        {
            logger.LogWarning("job {HeroJobId} is already completed", request.HeroJobId);
        }
        else
        {
            heroJob.Complete();
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}