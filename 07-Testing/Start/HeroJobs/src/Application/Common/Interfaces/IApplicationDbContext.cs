using Microsoft.EntityFrameworkCore;
using HeroJobs.Domain.HeroJobs;

namespace HeroJobs.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<HeroJob> HeroJobs { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}