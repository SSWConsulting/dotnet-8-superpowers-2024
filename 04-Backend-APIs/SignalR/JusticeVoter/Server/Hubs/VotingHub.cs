using JusticeVoter.Server.Services;
using JusticeVoter.Shared;
using Microsoft.AspNetCore.SignalR;

namespace JusticeVoter.Server.Hubs;

public class VotingHub : Hub
{
    public VotingHub(ILogger<VotingHub> logger, IHeroService heroService)
    {
        _logger = logger;
        _heroService = heroService;
    }

    private readonly ILogger<VotingHub> _logger;
    protected readonly IHeroService _heroService;

    public async Task SendVote(Vote vote)
    {
        _logger.LogInformation("Vote received: {heroName}, {voteId}", vote.HeroName, vote.Id);

        _heroService.AddVote(vote.Id);
        await Clients.All.SendAsync("Vote", vote);
    }

    public async Task ClearVotes()
    {
        await Clients.All.SendAsync("Refresh");
    }
}
