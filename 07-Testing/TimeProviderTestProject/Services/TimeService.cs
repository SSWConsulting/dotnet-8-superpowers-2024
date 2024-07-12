namespace TimeProviderTestProject.Services;

public class TimeService
{
    private readonly TimeProvider _timeProvider;

    public TimeService(TimeProvider timeProvider)
    {
        _timeProvider = timeProvider;
    }

    public DateTime GetCurrentTime()
    {
        return _timeProvider.GetUtcNow().DateTime;
    }

    public DateOnly GetCurrentDate()
    {
        return DateOnly.FromDateTime(_timeProvider.GetUtcNow().DateTime);
    }
}