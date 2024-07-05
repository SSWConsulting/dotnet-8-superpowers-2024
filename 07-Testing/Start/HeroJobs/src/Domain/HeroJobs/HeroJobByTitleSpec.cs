using Ardalis.Specification;

namespace HeroJobs.Domain.HeroJobs;

public sealed class HeroJobByTitleSpec : Specification<HeroJob>
{
    public HeroJobByTitleSpec(string title)
    {
        Query.Where(i => i.JobName == title);
    }
}