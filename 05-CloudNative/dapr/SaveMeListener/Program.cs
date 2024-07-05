using Dapr;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

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

app.MapPost("/save-me",
    [Topic("hero-pubsub", "need.help")]
    (RequestForHelp request, CancellationToken ct) =>
    {
        Console.WriteLine($"Hey {request.Name}! a hero is on their way to save you!!");
    })
    // Alternatively: .WithTopic("hero-pubsub", "need.help")
    .WithName("SaveMe")
    .WithOpenApi();

app.Run();

internal record RequestForHelp(string Name);