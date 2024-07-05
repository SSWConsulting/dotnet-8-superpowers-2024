using Application.UnitTests.Common;
using FluentAssertions;
using HeroJobs.Application.Features.HeroJobs.Commands.CompleteJob;
using HeroJobs.Domain.HeroJobs;
using HeroJobs.Infrastructure.Persistence;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Testing;

namespace Application.UnitTests.Features.HeroJobs;

public class CompleteHeroJobCommandTests : IDisposable, IAsyncDisposable
{
    private readonly ApplicationDbContext _dbContext = ApplicationDbContextFactory.CreateInMemory();

    [Fact]
    public async Task Handle_WithNewJob_LogsInfo()
    {
        // Arrange
        var job = HeroJob.Create("Save Gotham");
        _dbContext.Add(job);
        await _dbContext.SaveChangesAsync();
        var logger = new FakeLogger<CompleteHeroJobCommandHandler>();
        var sut = new CompleteHeroJobCommandHandler(_dbContext, logger);

        // Act
        await sut.Handle(new CompleteHeroJobCommand(job.Id.Value), CancellationToken.None);

        // Assert
        logger.Collector.Count.Should().Be(1);
        logger.Collector.GetSnapshot().First().Message.Should().StartWith("completing job");
    }

    [Fact]
    public async Task Handle_WithCompletedJob_ShouldLogWarning()
    {
        // Arrange
        var job = HeroJob.Create("Save Gotham");
        job.Complete();
        _dbContext.Add(job);
        await _dbContext.SaveChangesAsync();
        var logger = new FakeLogger<CompleteHeroJobCommandHandler>();
        var sut = new CompleteHeroJobCommandHandler(_dbContext, logger);

        // Act
        await sut.Handle(new CompleteHeroJobCommand(job.Id.Value), CancellationToken.None);

        // Assert
        logger.Collector.Count.Should().Be(2);
        logger.Collector.GetSnapshot()[0].Message.Should().StartWith("completing job");
        logger.Collector.GetSnapshot()[1].Level.Should().Be(LogLevel.Warning);
        logger.Collector.GetSnapshot()[1].Message.Should().Contain("already completed");
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await _dbContext.DisposeAsync();
    }
}