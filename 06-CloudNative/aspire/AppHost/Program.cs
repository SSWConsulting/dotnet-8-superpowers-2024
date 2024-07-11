using Aspire.Hosting.Dapr;

var builder = DistributedApplication.CreateBuilder(args);

// Add DAPR
var dapr = builder.AddDapr();

// ADD Custom Dapr Component that reads from a DAPR Component YAML file
//var stateStore = builder.AddDaprStateStore("statestore", new DaprComponentOptions()
//{
//    LocalPath = "../Components/cosmos-statestore.yaml"
//});

var pubSub = builder.AddDaprPubSub("pubsub", new DaprComponentOptions()
{
    LocalPath = "../DaprComponents/pubsub.yaml"
});


builder.AddProject<Projects.Producer>("producer")
    .WithExternalHttpEndpoints()
    .WithDaprSidecar(new DaprSidecarOptions
    {
        AppId = "producer",
        AppProtocol = builder.ExecutionContext.IsPublishMode ? null : "https"
    })
    //.WithReference(stateStore)
    .WithReference(pubSub);

builder.AddProject<Projects.Factory>("factory")
    .WithExternalHttpEndpoints()
    .WithDaprSidecar(new DaprSidecarOptions
    {
        AppId = "factory",
        AppProtocol = builder.ExecutionContext.IsPublishMode ? null : "https"
    })
    //.WithReference(stateStore)
    .WithReference(pubSub);

//// Executable to Host Dapr Dashboard - because we can
//var daprDashboard = builder.AddExecutable("dapr-dashboard", "dapr", ".", "dashboard")
//    .WithHttpEndpoint(port: 8080, targetPort: 8080, name: "dapr-dashboard-http", isProxied: false)
//    .ExcludeFromManifest();

builder.Build().Run();
