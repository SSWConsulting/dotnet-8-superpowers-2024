// See https://aka.ms/new-console-template for more information

using JusticeLeague.Client;
using JusticeLeague.Client.Models;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;

var builder = Host.CreateApplicationBuilder();
builder.Configuration.AddUserSecrets<Program>();

builder.Services.AddSingleton<IAccessTokenProvider, AccessTokenProvider>();
builder.Services.AddKeyedSingleton<HeroesClient>(HeroClientTypes.Anonymous, (p, o) =>
{
    var authProvider = new AnonymousAuthenticationProvider();
    var adapter = new HttpClientRequestAdapter(authProvider);
    return new HeroesClient(adapter);
});

builder.Services.AddKeyedSingleton<HeroesClient>(HeroClientTypes.Secure, (sp, o) =>
{
    var accessTokenProvider = sp.GetRequiredService<IAccessTokenProvider>();
    var authProvider = new BaseBearerTokenAuthenticationProvider(accessTokenProvider);
    var adapter = new HttpClientRequestAdapter(authProvider);
    return new HeroesClient(adapter);
});

builder.Services
    .AddOptionsWithValidateOnStart<ApiConfig>()
    .BindConfiguration(ApiConfig.SectionName)
    .ValidateDataAnnotations()
    ;

var app = builder.Build();

app.Start();

// Register user
var apiConfig = app.Services.GetRequiredService<IOptions<ApiConfig>>();
var anonymousClient = app.Services.GetRequiredKeyedService<HeroesClient>(HeroClientTypes.Anonymous);
await anonymousClient.Account.Register.PostAsync(new RegisterRequest
{
    Email = apiConfig.Value.UserName, Password = apiConfig.Value.Password
});

// Get Heroes
var secureClient = app.Services.GetRequiredKeyedService<HeroesClient>(HeroClientTypes.Secure);
var heroes = await secureClient.Heroes.GetAsync();
foreach (var hero in heroes)
{
    Console.WriteLine($"Hero - {hero.HeroName} ({hero.CivillianName})");
}

Console.ReadKey();