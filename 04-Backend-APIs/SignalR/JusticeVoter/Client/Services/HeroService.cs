using JusticeVoter.Shared;
using System.Net.Http.Json;

namespace JusticeVoter.Client.Services;

public class HeroService
{
    private readonly HttpClient _http;

    public HeroService(HttpClient http)
    {
        _http = http;
    }

    public async Task<Hero[]> GetHeroes(string hero)
    {
        return await _http.GetFromJsonAsync<Hero[]?>($"/Heroes/{hero}") ?? [];
    }

    public async Task<int> GetHeroVoteTally(int heroId)
    {
        return await _http.GetFromJsonAsync<int?>($"/Votes/{heroId}") ?? 0;
    }
}
