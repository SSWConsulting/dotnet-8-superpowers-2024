using JusticeVoter.Shared;

namespace JusticeVoter.Server.Services;

public interface IHeroService
{
    List<Hero> GetHeroes(string hero);
    void AddVote(int heroId);
    int GetVoteTally(int heroId);
    void ClearVotes();
}
