namespace HeroJobs.Application.Features.HeroJobs.Commands.CompleteJob;

public class CompleteHeroJobCommandValidator : AbstractValidator<CompleteHeroJobCommand>
{
    public CompleteHeroJobCommandValidator()
    {
        RuleFor(p => p.HeroJobId)
            .NotEmpty();
    }
}