using HeroJobs.Application.Common.Interfaces;
using HeroJobs.Domain.HeroJobs;
using Microsoft.EntityFrameworkCore;

namespace HeroJobs.Application.Features.HeroJobs.Commands.CreateJob;

public class CreateHeroJobCommandValidator : AbstractValidator<CreateHeroJobCommand>
{
    private readonly IApplicationDbContext _dbContext;

    public CreateHeroJobCommandValidator(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(p => p.Title)
            .NotEmpty()
            .MaximumLength(200)
            .MustAsync(BeUniqueTitle).WithMessage("'{PropertyName}' must be unique");
    }

    // TODO DM: Consider pushing this business validation to the Domain
    private async Task<bool> BeUniqueTitle(string title, CancellationToken cancellationToken)
    {
        var spec = new HeroJobByTitleSpec(title);

        var exists = await _dbContext.HeroJobs
            .WithSpecification(spec)
            .AnyAsync(cancellationToken: cancellationToken);

        return !exists;
    }
}