# Hero Jobs - Unit & Integration Testing Demo

Demo of unit and integration tests.  Originally created from the SSW CA template - https://github.com/SSWConsulting/SSW.CleanArchitecture

## Pre-requisites

- WSL2
- Docker Desktop
- .NET 8

## Project Structure

- Run through functionality
- Brief overview of project structure

## Unit Tests

- Show `HeroJobTests`
- Show `HeroJobByTitleSpecTests`

## Integration Tests

- Show `CreateHeroJobEndpointTests`
- Show Rider Dependency Diagram for base types
- Walk through base type code
- Show tests running
- Pause tests and connect to DB container

## Logging Tests

Create the `Application.UnitTests` project

```bash
cd Tests
dotnet new xunit -n Application.UnitTests
cd Application.UnitTests
dotnet add reference ../src/Application
```

Add the testing package to the `Test` project

```bash
dotnet add package Microsoft.Extensions.Diagnostics.Testing
````

Add NSubstitute to the testing project

```bash
dotnet add package NSubstitute
```

Add fluent assertions to the testing project

```bash
dotnet add package FluentAssertions
```

Add logging to `CompleteHeroJobCommandHandler`

```csharp
// updated 👇
public class CompleteHeroJobCommandHandler(IApplicationDbContext dbContext, ILogger<CompleteHeroJobCommandHandler> logger) : IRequestHandler<CompleteHeroJobCommand>
// updated 👆
{
    public async Task Handle(CompleteHeroJobCommand request, CancellationToken cancellationToken)
    {
        // added 👇
        logger.LogInformation("completing job {HeroJobId}", request.HeroJobId);
        // added 👆

        var heroJobId = new HeroJobId(request.HeroJobId);

        var heroJob = await dbContext.HeroJobs
            .WithSpecification(new HeroJobByIdSpec(heroJobId))
            .FirstOrDefaultAsync(cancellationToken);

        if (heroJob is null)
            throw new NotFoundException(nameof(heroJob), heroJobId);

        // added 👇
        if (heroJob.Done)
        {
            logger.LogWarning("job {HeroJobId} is already completed", request.HeroJobId);
        }
        else
        {
            heroJob.Complete();
        }
        // added 👆

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
```

Create an in-memory EF Core provider

```csharp
public static class ApplicationDbContextFactory
{
    public static ApplicationDbContext CreateInMemory()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var currentUserService = Substitute.For<ICurrentUserService>();
        var dateTime = Substitute.For<IDateTime>();
        var mediator = Substitute.For<IMediator>();
        var entitySaveChangesInterceptor = new EntitySaveChangesInterceptor(currentUserService, dateTime);
        var dispatchDomainEventsInterceptor = new DispatchDomainEventsInterceptor(mediator);
        var dbContext =
            new ApplicationDbContext(options, entitySaveChangesInterceptor, dispatchDomainEventsInterceptor);

        dbContext.Database.EnsureCreated();

        return dbContext;
    }
}
```


Create the tests `Features/HeroJobs/CompleteHeroJobCommandTests.cs`

```csharp
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
```

> **NOTE**: the logger will contain a collection of all the logs that have been added during execution

Run the tests and show them working

## Time Tests

Update the `HeroJob` so that we can check with a job is 'due'

```csharp
public bool IsReminderDue(TimeProvider timeProvider)
{
    return Reminder <= timeProvider.GetUtcNow();
}
```

Add the following packages to the `Test` project

```bash
dotnet add package Microsoft.Extensions.TimeProvider.Testing
```

Create the tests in `Domain.UnitTests` in the existing `HeroJobTests` file

```csharp
[Fact]
public void IsReminderDue_WhenReminderHasNotPast_ReturnsFalse()
{
    // Arrange
    var now = DateTime.UtcNow;
    var reminder = now.AddDays(1);
    var timeProvider = new FakeTimeProvider();
    timeProvider.SetUtcNow(now);
    var sut = HeroJob.Create("title", "note", PriorityLevel.High, reminder);

    // Act
    var result = sut.IsReminderDue(timeProvider);

    // Assert
    result.Should().BeFalse();

    // NOTE: Not ideal to test two different things at once, but this is a simple way to show how the time provider works
    timeProvider.Advance(TimeSpan.FromDays(1));
    result = sut.IsReminderDue(timeProvider);
    result.Should().BeTrue();
}
```

Run the tests and show they pass
