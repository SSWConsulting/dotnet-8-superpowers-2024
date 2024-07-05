using HeroJobs.Domain.Common.Base;

namespace HeroJobs.Domain.HeroJobs;

// For strongly typed IDs, check out the rule: https://www.ssw.com.au/rules/do-you-use-strongly-typed-ids/
public readonly record struct HeroJobId(Guid Value);

public class HeroJob : BaseEntity<HeroJobId>
{
    // NOTE: private setters for behavior we want to encapsulate, and public setters for properties that don't have behavior

    public string? JobName { get; private set; }
    public string? Note { get; set; }
    public PriorityLevel Priority { get; set; }
    public DateTime Reminder { get; set; }
    public bool Done { get; private set; }

    // Needed for EF
    private HeroJob() { }

    public static HeroJob Create(string title)
    {
        ArgumentException.ThrowIfNullOrEmpty(title, nameof(title));

        var HeroJob = new HeroJob
        {
            JobName = title,
            Priority = PriorityLevel.None,
            Done = false
        };

        HeroJob.AddDomainEvent(new HeroJobCreatedEvent(HeroJob));

        return HeroJob;
    }

    public static HeroJob Create(string title, string note, PriorityLevel priority, DateTime reminder)
    {
        var HeroJob = Create(title);
        HeroJob.Note = note;
        HeroJob.Priority = priority;
        HeroJob.Reminder = reminder;

        return HeroJob;
    }

    public void Complete()
    {
        Done = true;

        AddDomainEvent(new HeroJobCompletedEvent(this));
    }
}