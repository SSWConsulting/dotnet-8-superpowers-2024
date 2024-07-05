using HeroJobs.Domain.Common.Base;

namespace HeroJobs.Domain.HeroJobs;

public record HeroJobCreatedEvent(HeroJob Item) : DomainEvent;