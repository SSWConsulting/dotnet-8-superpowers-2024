using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HeroJobs.Domain.HeroJobs;
using Bogus;

namespace HeroJobs.Infrastructure.Persistence;

public class ApplicationDbContextInitializer
{
    private readonly ILogger<ApplicationDbContextInitializer> _logger;
    private readonly ApplicationDbContext _dbContext;

    private const int NumHeroJobs = 20;

    public ApplicationDbContextInitializer(ILogger<ApplicationDbContextInitializer> logger, ApplicationDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task InitializeAsync()
    {
        try
        {
            if (_dbContext.Database.IsSqlServer())
            {
                await _dbContext.Database.MigrateAsync();
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while migrating or initializing the database");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            if (_dbContext.HeroJobs.Any())
                return;

            var faker = new Faker<HeroJob>()
                .CustomInstantiator(f => HeroJob.Create(
                    f.Lorem.Sentence(3), f.Lorem.Sentence(10), f.Random.Enum<PriorityLevel>(), f.Date.Future(1, DateTime.UtcNow)));

            var HeroJobs = faker.Generate(NumHeroJobs);
            await _dbContext.HeroJobs.AddRangeAsync(HeroJobs);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while seeding the database");
            throw;
        }
    }
}