using HeroJobs.Domain.Common.Base;

namespace HeroJobs.Domain.HeroJobs;

public record HeroJobCompletedEvent(HeroJob Item) : DomainEvent;