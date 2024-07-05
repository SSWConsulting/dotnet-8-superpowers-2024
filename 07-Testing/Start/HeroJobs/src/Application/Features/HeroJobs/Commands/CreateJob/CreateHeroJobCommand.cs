using HeroJobs.Application.Common.Interfaces;

namespace HeroJobs.Application.Features.HeroJobs.Commands.CreateJob;

public record CreateHeroJobCommand(string Title) : IRequest<Guid>;

// ReSharper disable once UnusedType.Global
public class CreateHeroJobCommandHandler : IRequestHandler<CreateHeroJobCommand, Guid>
{
    private readonly IApplicationDbContext _dbContext;

    public CreateHeroJobCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> Handle(CreateHeroJobCommand request, CancellationToken cancellationToken)
    {
        var HeroJob = Domain.HeroJobs.HeroJob.Create(request.Title!);

        await _dbContext.HeroJobs.AddAsync(HeroJob, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return HeroJob.Id.Value;
    }
}