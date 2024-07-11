using Dapr.Client;
using Microsoft.AspNetCore.Mvc;

using Shared;

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


app.MapPost("/produce", async (
    [FromBody] ProduceItemRequest item,
    [FromServices] DaprClient client) =>
{
    try
    {
        Console.WriteLine($"Request received to produce {item.Quantity}x {item.Name}" + (item.Quantity > 1 ? "s" : ""));
        
        for(int j = 0; j < item.Quantity; j++)
        {
            var msg = new ProduceItemMessage(item.Name, DateTime.Now);
            await client.PublishEventAsync("factory-pubsub", "item-queue", item);
        }

        return Results.Ok();
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex);
    }
})
.WithName("Produce")
.WithOpenApi();

app.Run();

