namespace HeroJobs.Application.Common.Interfaces;

public interface IDateTime
{
    // TODO: Talk to Gordon about this - System Clock (https://github.com/SSWConsulting/HeroJobs/issues/77)
    public DateTimeOffset Now => DateTimeOffset.UtcNow;
}