using HeroJobs.Domain.HeroJobs;

namespace HeroJobs.Application.Features.HeroJobs.Queries.GetAllJobs;

public class GetAllHeroJobsMapping : Profile
{
    public GetAllHeroJobsMapping()
    {
        CreateMap<HeroJob, HeroJobDto>()
            .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id.Value));
    }
}