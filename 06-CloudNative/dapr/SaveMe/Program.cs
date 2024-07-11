using Dapr.Client;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

//builder.AddServiceDefaults();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDaprClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCloudEvents();
app.MapSubscribeHandler();

app.UseHttpsRedirection();


app.MapPost("/save-me", async (
    [FromBody] RequestForHelp item, 
    [FromServices] DaprClient client) =>
{
    try
    {
        Console.WriteLine($"{item.Name} is asking for help... ill let the heroes know!");
        await client.PublishEventAsync("hero-pubsub", "need.help", item);
        return Results.Ok();
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex);
    }
})
.WithName("SaveMe")
.WithOpenApi();

//app.MapDefaultEndpoints();

app.Run();

internal record RequestForHelp(string Name);