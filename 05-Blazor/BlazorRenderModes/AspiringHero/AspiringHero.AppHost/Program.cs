var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedisContainer("cache");

var apiservice = builder.AddProject<Projects.AspiringHero_ApiService>("apiservice");

var justiceLeagueApi = builder.AddProject<Projects.JusticeLeague_Api>("justiceleague.api");

var serverFrontend = builder.AddProject<Projects.AspiringHero_Web>("serverfrontend")
    .WithReference(cache)
    .WithReference(apiservice)
    .WithReference(justiceLeagueApi);

//builder.AddDapr();

//builder.AddProject<Projects.SaveMe>("saveme")
//    .WithDaprSidecar("save-me-api");

//builder.AddProject<Projects.SaveMeListener>("savemelistener")
//    .WithDaprSidecar("save-me-listener-api");

builder.Build().Run();
