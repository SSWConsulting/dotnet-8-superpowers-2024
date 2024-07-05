using HeroJobs.Domain.HeroJobs;

namespace HeroJobs.Domain.UnitTests.HeroJobs;

public class HeroJobTests
{
    // Rule naming convention: Method/Class_Condition_Result
    // as per https://www.ssw.com.au/rules/follow-naming-conventions-for-tests-and-test-projects/

    [Fact]
    public void Create_WithValidTitle_ShouldSucceed()
    {
        // Arrange
        var title = "title";

        // Act
        var heroJob = HeroJob.Create(title);

        // Assert
        heroJob.Should().NotBeNull();
        heroJob.JobName.Should().Be(title);
        heroJob.Priority.Should().Be(PriorityLevel.None);
    }

    [Fact]
    public void Create_WithNullTitle_ShouldThrow()
    {
        // Arrange
        string? title = null;

        // Act
        Action act = () => HeroJob.Create(title!);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Value cannot be null. (Parameter 'title')");
    }

    [Fact]
    public void Create_ShouldRaiseDomainEvent()
    {
        // Act
        var heroJob = HeroJob.Create("title");

        // Assert
        heroJob.DomainEvents.Should().NotBeNull();
        heroJob.DomainEvents.Should().HaveCount(1);
        heroJob.DomainEvents.Should().ContainSingle(x => x is HeroJobCreatedEvent);
    }

    [Fact]
    public void Complete_ShouldSetDone()
    {
        // Arrange
        var heroJob = HeroJob.Create("title");

        // Act
        heroJob.Complete();

        // Assert
        heroJob.Done.Should().BeTrue();
    }

    [Fact]
    public void Complete_ShouldRaiseDomainEvent()
    {
        // Arrange
        var heroJob = HeroJob.Create("title");

        // Act
        heroJob.Complete();

        // Assert
        heroJob.DomainEvents.Should().NotBeNull();
        heroJob.DomainEvents.Should().HaveCount(2);
        heroJob.DomainEvents.Should().ContainSingle(x => x is HeroJobCompletedEvent);
    }
}