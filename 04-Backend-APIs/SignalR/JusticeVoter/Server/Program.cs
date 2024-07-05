using JusticeVoter.Server.Hubs;
using JusticeVoter.Server.Services;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddSingleton<IHeroService, HeroService>();
builder.Services.AddSingleton<VotingHub>();

builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSwagger();
app.UseSwaggerUI(s =>
{
    s.SwaggerEndpoint("/swagger/v1/swagger.json", "Justice Voter API v1");
});

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.MapHub<VotingHub>("/vote");
app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.MapGet("/heroes/{hero}", (string hero, [FromServices] IHeroService _heroService) =>
{
    try
    {
        return Results.Ok(_heroService.GetHeroes(hero));
    }
    catch
    {
        return Results.NotFound(hero);
    }
});

app.MapGet("/votes/{heroId}", (int heroId, [FromServices] IHeroService _heroService) =>
{
    try
    {
        return Results.Ok(_heroService.GetVoteTally(heroId));
    }
    catch
    {
        return Results.BadRequest(heroId);
    }
});

app.MapPost("/votes/clear", async ([FromServices]IHeroService _heroService, [FromServices]VotingHub _hub) =>
{
    try
    {
        _heroService.ClearVotes();
        await _hub.ClearVotes();
        return Results.Ok();
    }
    catch
    {
        return Results.BadRequest();
    }
});

app.Run();
