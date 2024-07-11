using BlazorRenderModes;
using BlazorRenderModes.Client.Pages;
using BlazorRenderModes.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

//builder.Services.AddHttpClient<WeatherApiClient>(client => client.BaseAddress = new("http://localhost:5117"));
builder.Services.AddHttpClient<WeatherApiClient>(client => client.BaseAddress = new("https://localhost:7281"));
//builder.Services.AddHttpClient<WeatherApiClient>(client => client.BaseAddress = new("http://apiservice"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(BlazorRenderModes.Client._Imports).Assembly);

app.Run();
