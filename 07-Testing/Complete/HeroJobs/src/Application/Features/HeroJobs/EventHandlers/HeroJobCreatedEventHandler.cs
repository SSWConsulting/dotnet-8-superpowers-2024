using Microsoft.Extensions.Logging;
using HeroJobs.Domain.HeroJobs;

namespace HeroJobs.Application.Features.HeroJobs.EventHandlers;

public class HeroJobCreatedEventHandler : INotificationHandler<HeroJobCreatedEvent>
{
    private readonly ILogger<HeroJobCreatedEventHandler> _logger;

    public HeroJobCreatedEventHandler(ILogger<HeroJobCreatedEventHandler> logger) => _logger = logger;

    public Task Handle(HeroJobCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("HeroJobCreatedEventHandler: {JobName} was created", notification.Item.JobName);

        return Task.CompletedTask;
    }
}