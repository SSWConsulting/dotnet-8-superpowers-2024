using Dapr;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc;

using Shared;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddDaprClient();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCloudEvents();
app.MapSubscribeHandler();

app.UseHttpsRedirection();

app.MapPost("/make-item",
    [Topic("factory-pubsub", "item-queue")]
    (ProduceItemMessage msg, CancellationToken ct) =>
    {
        Console.WriteLine($"Ok, factory is making a {msg.Name} that was requested at {msg.RequestedAt}");
    })
    // Alternatively: .WithTopic("factory-pubsub", "item-queue")
    .WithName("MakeItem")
    .WithOpenApi();

app.Run();
