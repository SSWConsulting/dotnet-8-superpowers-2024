using AspiringHero.Client.Pages;
using AspiringHero.Web;
using AspiringHero.Web.Components;
using JusticeLeague.Client;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using System.Net.Http;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();
builder.AddRedisOutputCache("cache");

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddHttpClient<WeatherApiClient>(client=> client.BaseAddress = new("http://apiservice"));

builder.Services.AddHttpClient("JusticeLeague.Http", client => client.BaseAddress = new("http://justiceleague.api"));

builder.Services.AddKeyedScoped<JusticeLeagueClient>("Anonymous", (p, o) =>
{
    IHttpClientFactory httpClientFactory = p.GetRequiredService<IHttpClientFactory>();
    var httpClient = httpClientFactory.CreateClient("JusticeLeague.Http");
    var authProvider = new AnonymousAuthenticationProvider();
    var adapter = new HttpClientRequestAdapter(authProvider, httpClient: httpClient);
    return new JusticeLeagueClient(adapter);
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseWebAssemblyDebugging();
}

app.UseStaticFiles();
app.UseAntiforgery();
app.UseOutputCache();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Counter).Assembly);

app.MapDefaultEndpoints();

app.Run();
