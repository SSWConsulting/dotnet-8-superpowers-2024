using Microsoft.Extensions.Time.Testing;
using TimeProviderTestProject.Services;

namespace TimeProviderTestProject.Tests;

public class TimeServiceTests
{
    [Fact]
    public void GetCurrentTime_ReturnsMockedTime()
    {
        // Arrange
        var fakeTimeProvider = new FakeTimeProvider(new DateTimeOffset(new DateTime(2023, 1, 1)));
        var timeService = new TimeService(fakeTimeProvider);

        // Act
        var currentTime = timeService.GetCurrentTime();

        // Assert
        Assert.Equal(new DateTime(2023, 1, 1), currentTime);
    }

    [Fact]
    public void GetCurrentDate_ReturnsMockedDate()
    {
        // Arrange
        var fakeTimeProvider = new FakeTimeProvider(new DateTimeOffset(new DateTime(2023, 1, 1)));
        var timeService = new TimeService(fakeTimeProvider);

        // Act
        var currentDate = timeService.GetCurrentDate();

        // Assert
        Assert.Equal(new DateOnly(2023, 1, 1), currentDate);
    }
}