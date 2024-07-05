using HeroJobs.Application.Common.Exceptions;
using HeroJobs.Application.Common.Interfaces;
using HeroJobs.Domain.HeroJobs;
using Microsoft.EntityFrameworkCore;

namespace HeroJobs.Application.Features.HeroJobs.Commands.CompleteJob;

public record CompleteHeroJobCommand(Guid HeroJobId) : IRequest;

// ReSharper disable once UnusedType.Global
public class CompleteHeroJobCommandHandler : IRequestHandler<CompleteHeroJobCommand>
{
    private readonly IApplicationDbContext _dbContext;

    public CompleteHeroJobCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(CompleteHeroJobCommand request, CancellationToken cancellationToken)
    {
        var HeroJobId = new HeroJobId(request.HeroJobId);

        var HeroJob = await _dbContext.HeroJobs
            .WithSpecification(new HeroJobByIdSpec(HeroJobId))
            .FirstOrDefaultAsync(cancellationToken);

        if (HeroJob is null)
            throw new NotFoundException(nameof(HeroJob), HeroJobId);

        HeroJob.Complete();

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}