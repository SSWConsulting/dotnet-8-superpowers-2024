---
runme:
  id: 01HGS1DB3SW2F3P042JR5KZQDV
  version: v2.0
---

# GRPC Hero

## Grpc Hello World

1. Launch VS and Create new Project

2. Select ASP.NET Core gRPC Service

3. Rename `greet.proto` to `hero.proto`

4. Show `hero.proto`

5. Show `HeroService`

6. Add new method and rebuild and override in `HeroService`

7. Add new stream method to the Service in `.proto`

```csharp

// IN THE SERVICE DEFINITION

// Sends a stream of hero's greetings to a user
rpc SayHelloStream (HelloStreamRequest) returns (stream HelloReply);

// NEW MESSAGE CONTRACT

// The request message containing the fan's name and number of greetings.
message HelloStreamRequest {
    // Your name
    string name = 1;
    // Number of greetings
    int32 count = 2;
}
```

8. Add override for new `SayHelloStream` method:

```csharp
public override async Task SayHelloStream(
    HelloStreamRequest request, 
    IServerStreamWriter<HelloReply> responseStream, 
    ServerCallContext context)
{
    if (request.Count <= 0)
    {
        throw new RpcException(new Status(StatusCode.InvalidArgument, "Count must be greater than zero."));
    }

    _logger.LogInformation($"Sending {request.Count} hellos to {request.Name}");

    for (var i = 0; i < request.Count; i++)
    {
        await responseStream.WriteAsync(new HelloReply { Message = $"Hello {request.Name} {i + 1}" });
        await Task.Delay(TimeSpan.FromSeconds(1));
    }
}
```

## Grpc Reflection

1. Add a `Grpc.AspNetCore.Server.Reflection` package reference.

```sh
dotnet add package Grpc.AspNetCore.Server.Reflection
```

2. Register reflection in `Program.cs`:

`AddGrpcReflection` to register services that enable reflection.
`MapGrpcReflectionService` to add a reflection service endpoint.

```csharp
using HeroRelay.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
// 👇👇👇
builder.Services.AddGrpcReflection();
// 👆👆👆

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
// 👇👇👇
app.MapGrpcReflectionService();
// 👆👆👆

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client!");

app.Run();
```

3. When gRPC reflection is set up:

   - A gRPC reflection service is added to the server app.
   - Client apps that support gRPC reflection can call the reflection service to discover services hosted by the server.
   - gRPC services are still called from the client. Reflection only enables service discovery and doesn't bypass server-side security. Endpoints protected by authentication and authorization require the caller to pass credentials for the endpoint to be called successfully.

4. Launch GRPC Hero app as well as Postman or Insomnia

5. Connect to the base address of GRPC Hero, use Server Reflection to discover services and models

6. Run sample queries

## Grpc JsonTranscoding

1. Add a package reference to `Microsoft.AspNetCore.Grpc.JsonTranscoding`

```shell
dotnet add package Microsoft.AspNetCore.Grpc.JsonTranscoding
```

2. Register transcoding in server startup code by adding `AddJsonTranscoding`:

   - In the `Program.cs` file, change `builder.Services.AddGrpc();` to `builder.Services.AddGrpc().AddJsonTranscoding();`.

3. Add `<IncludeHttpRuleProtos>true</IncludeHttpRuleProtos>` to the property group in the `.csproj` project file:

```xml {"id":"01HGS1DB3SW2F3P042JAS1AE9Q"}
<Project Sdk="Microsoft.NET.Sdk.Web">

<PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
    <InvariantGlobalization>true</InvariantGlobalization>
    <IncludeHttpRuleProtos>true</IncludeHttpRuleProtos>
</PropertyGroup>

```

4. Annotate gRPC methods in your `.proto` files with HTTP bindings and routes:

```groovy

// 👇👇👇
import "google/api/annotations.proto";
// 👆👆👆

service Hero {
    rpc SayHello (HelloRequest) returns (HelloReply) {
        // 👇👇👇
        option (google.api.http) = {
            get: "/v1/hero/{name}"
        };
        // 👆👆👆
    }

    rpc SayHelloStream (HelloStreamRequest) returns (stream HelloReply) {
        // 👇👇👇
        option (google.api.http) = {
            post: "/v1/hero"
            body: "*"
        };
        // 👆👆👆
    }
}
```

## GrpcSwagger

1. Add a package reference to `Microsoft.AspNetCore.Grpc.Swagger`

```sh
dotnet add package Microsoft.AspNetCore.Grpc.Swagger
```

2. Add Swagger to services

```csharp
using HeroRelay.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc().AddJsonTranscoding();
builder.Services.AddGrpcReflection();

// 👇👇👇
builder.Services.AddGrpcSwagger();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1",
        new OpenApiInfo { Title = "gRPC Hero transcoding", Version = "v1" });
});
// 👆👆👆

var app = builder.Build();

// Configure the HTTP request pipeline.

// 👇👇👇 - Add Swagger UI Middleware
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My Hero API V1");
});
// 👆👆👆

app.MapGrpcService<GreeterService>();
app.MapGrpcReflectionService();

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
```

3. Enable the XML documentation file in the server project with:

```xml
<GenerateDocumentationFile>true</GenerateDocumentationFile>
```

4. Configure `AddSwaggerGen` to read the generated XML file. Pass the XML file path to `IncludeXmlComments` and `IncludeGrpcXmlComments`, as in the following example:

```csharp
var filePath = Path.Combine(System.AppContext.BaseDirectory, "GrpcHero.xml");
c.IncludeXmlComments(filePath);
c.IncludeGrpcXmlComments(filePath, includeControllerXmlComments: true);
```

## Docs

- Grpc Reflection - https://learn.microsoft.com/en-us/aspnet/core/grpc/test-tools?view=aspnetcore-8.0
- Json Transcoding: https://learn.microsoft.com/en-us/aspnet/core/grpc/json-transcoding?view=aspnetcore-8.0
- Http Rules: https://learn.microsoft.com/en-us/aspnet/core/grpc/json-transcoding-binding?view=aspnetcore-8.0
- Grpc Swagger/OpenAPI - https://learn.microsoft.com/en-us/aspnet/core/grpc/json-transcoding-openapi?view=aspnetcore-8.0