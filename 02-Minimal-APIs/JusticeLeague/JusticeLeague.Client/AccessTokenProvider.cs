using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Kiota.Abstractions.Authentication;

namespace JusticeLeague.Client;

public class AccessTokenProvider(
    IOptions<ApiConfig> apiConfig,
    [FromKeyedServices(HeroClientTypes.Anonymous)]
    HeroesClient client)
    : IAccessTokenProvider
{
    public async Task<string> GetAuthorizationTokenAsync(
        Uri requestUri,
        Dictionary<string, object>? options = default,
        CancellationToken cancellationToken = default)
    {
        var token = await client.Account.Login.PostAsync(
            new() { Email = apiConfig.Value.UserName, Password = apiConfig.Value.Password },
            cancellationToken: cancellationToken);
        return token?.AccessToken ?? string.Empty;
    }

    public AllowedHostsValidator AllowedHostsValidator { get; } = new();
}