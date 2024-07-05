using Microsoft.EntityFrameworkCore;
using HeroJobs.Application.Common.Interfaces;

namespace HeroJobs.Application.Features.EntityNames.Queries.QueryName;

public record QueryNameQuery : IRequest<IReadOnlyList<EntityNameDto>>;

public class QueryNameQueryHandler : IRequestHandler<QueryNameQuery, IReadOnlyList<EntityNameDto>>
{
    private readonly IMapper _mapper;
    private readonly IApplicationDbContext _dbContext;

    public QueryNameQueryHandler(
        IMapper mapper,
        IApplicationDbContext dbContext)
    {
        _mapper = mapper;
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<EntityNameDto>> Handle(
        QueryNameQuery request,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}