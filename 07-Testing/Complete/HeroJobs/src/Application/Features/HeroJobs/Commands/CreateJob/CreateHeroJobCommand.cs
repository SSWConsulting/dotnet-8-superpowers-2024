using HeroJobs.Application.Common.Interfaces;

namespace HeroJobs.Application.Features.HeroJobs.Commands.CreateJob;

public record CreateHeroJobCommand(string Title) : IRequest<Guid>;

// ReSharper disable once UnusedType.Global
public class CreateHeroJobCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<CreateHeroJobCommand, Guid>
{
    public async Task<Guid> Handle(CreateHeroJobCommand request, CancellationToken cancellationToken)
    {
        var HeroJob = Domain.HeroJobs.HeroJob.Create(request.Title!);

        await dbContext.HeroJobs.AddAsync(HeroJob, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return HeroJob.Id.Value;
    }
}