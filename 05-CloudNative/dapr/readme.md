# DAPR

DAPR (Distributed APplication Runtime) provides a simple interface for developers to use when writing distributed ("microservices") applications.

DAPR Building blocks cover most scenarios encountered when writing distributed applications such as Service to Service communication, State Management, PubSub, Secrets, Configuiration, Locks, etc.

DAPR runs on a Sidecar Architecture which means your application only needs to communicate with a Sidecar to take advantage of all the DAPR building blocks and does not need to take on dependencies to actual infrastructure for things like Azure Service Bus, Redis, Azure Storage, S3 Buckets, KeyVault, etc. The Sidecar has all these dependencies and it is removed from your own applications.

To get started with DAPR, install the CLI and initialize it for your local development.

## Installing Dapr CLI

```sh
winget install Dapr.CLI
```

## Initialize Dapr for local development

The Dapr placement engine and Redis (and ZipKin) will start as containers

```sh
dapr init
```

## Create a Dapr app

1. Create a new WebAPI application

  ```sh
  dotnet new webapi -n myDaprApp
  ```

1. Add the DAPR packages

  ```sh
  dotnet add package Dapr.Client 
  dotnet add package Dapr.AspNetCore
  ```

1. Add the Dapr Client to the start of your application

  ```csharp
  builder.Services.AddDaprClient();
  ```

1. Add the Dapr CloudEvents and PubSub subscribers/listeners

  ```csharp
  app.UseCloudEvents();
  app.MapSubscribeHandler();
  ```

## Create a Multi-app run file

The run file allows you to specify common configuration that applies too all services, as well as specifying app specific settings.

```yaml
version: 1
common: # optional section for variables shared across apps
  resourcesPath: ./DaprComponents # any dapr resources to be shared across apps
  appLogDestination: fileAndConsole # (optional), can be file, console or fileAndConsole. default is fileAndConsole.
  daprdLogDestination: file # (optional), can be file, console or fileAndConsole. default is file.
  appSSL: true
  env:  # any environment variable shared across apps
    DEBUG: true
apps:
  - appID: save-me-api
    appDirPath: ./SaveMe/
    appPort: 7150
    daprHTTPPort: 3500
    command: ["dotnet", "run", "SaveMe.csproj", "-lp", "https"]
  - appID: save-me-listener-api
    appDirPath: ./SaveMeListener/
    appPort: 7264
    daprHTTPPort: 3501
    command: ["dotnet", "run", "SaveMeListener.csproj", "-lp", "https"]
```

## Run all your services at once

```sh
dapr run -f .\dapr.yaml
```

## Deploy DAPR apps to Azure

Check out my blog: https://github.com/william-liebenberg/practical-dapr

DONE!