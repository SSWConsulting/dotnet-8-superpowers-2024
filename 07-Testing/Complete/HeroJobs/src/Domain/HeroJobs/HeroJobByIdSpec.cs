using Ardalis.Specification;

namespace HeroJobs.Domain.HeroJobs;

public sealed class HeroJobByIdSpec : SingleResultSpecification<HeroJob>
{
    public HeroJobByIdSpec(HeroJobId HeroJobId)
    {
        Query.Where(t => t.Id == HeroJobId);
    }
}