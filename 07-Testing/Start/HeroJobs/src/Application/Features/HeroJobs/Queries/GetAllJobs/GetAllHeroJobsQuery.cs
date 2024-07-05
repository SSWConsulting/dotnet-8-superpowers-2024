using AutoMapper.QueryableExtensions;
using HeroJobs.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HeroJobs.Application.Features.HeroJobs.Queries.GetAllJobs;

public record GetAllHeroJobsQuery : IRequest<IReadOnlyList<HeroJobDto>>;

public class GetAllHeroJobsQueryHandler : IRequestHandler<GetAllHeroJobsQuery, IReadOnlyList<HeroJobDto>>
{
    private readonly IMapper _mapper;
    private readonly IApplicationDbContext _dbContext;

    public GetAllHeroJobsQueryHandler(
        IMapper mapper,
        IApplicationDbContext dbContext)
    {
        _mapper = mapper;
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<HeroJobDto>> Handle(
        GetAllHeroJobsQuery request,
        CancellationToken cancellationToken)
    {
        return await _dbContext.HeroJobs
            .ProjectTo<HeroJobDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}