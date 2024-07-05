using HeroJobs.Domain.Common.Base;

namespace HeroJobs.Domain.HeroJobs;

// For strongly typed IDs, check out the rule: https://www.ssw.com.au/rules/do-you-use-strongly-typed-ids/
public readonly record struct HeroJobId(Guid Value);

public class HeroJob : BaseEntity<HeroJobId>
{
    // NOTE: private setters for behavior we want to encapsulate, and public setters for properties that don't have behavior

    public string? JobName { get; private set; }
    public string? Note { get; private set; }
    public PriorityLevel Priority { get; private set; }
    public DateTime Reminder { get; private set; }
    public bool Done { get; private set; }

    // Needed for EF
    private HeroJob() { }

    public static HeroJob Create(string title)
    {
        ArgumentException.ThrowIfNullOrEmpty(title, nameof(title));

        var heroJob = new HeroJob
        {
            JobName = title,
            Priority = PriorityLevel.None,
            Done = false
        };

        heroJob.AddDomainEvent(new HeroJobCreatedEvent(heroJob));

        return heroJob;
    }

    public static HeroJob Create(string title, string note, PriorityLevel priority, DateTime reminder)
    {
        var heroJob = Create(title);
        heroJob.Note = note;
        heroJob.Priority = priority;
        heroJob.Reminder = reminder;

        return heroJob;
    }

    public void Complete()
    {
        Done = true;

        AddDomainEvent(new HeroJobCompletedEvent(this));
    }

    public bool IsReminderDue(TimeProvider timeProvider)
    {
        return Reminder <= timeProvider.GetUtcNow();
    }
}