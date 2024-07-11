# .NET Aspire

## Setting up Aspire

```sh
dotnet workload install aspire
dotnet workload update aspire
```

## Create an Aspire Application

1. Open VS
2. Create New Project
3. Select `Aspire Starter Application`
4. Make sure the `AppHost` project is set as the startup project
5. Add a new ASP.NET Core WebAPI project (call it `HeroService`)
6. Right-click `HeroService` project | `Add` | `Add .NET Aspire Orchestrator Support...`
7. F5 to run the project
8. Explore the dashboard
   1. Projects
   2. Logs
   3. Traces
   4. Metrics


## Deploying Aspire app to Azure

Use the Azure Developer CLI (AZD CLI) to provision resources in Azure and deploy the compiled application to those resources. Aspire Apps are deployed as Azure Container Apps. The AZD CLI uses a manifest (that is generated from the AppHost project) to figure out what resources to create in Azure Container Apps and how apps are connected to each other.

### Installing Azure Developer CLI

```sh
winget install Microsoft.Azd
winget uninstall Microsoft.Azd
winget upgrade Microsoft.Azd
```


### Generate the Manifest

The manifest file that describes how all the services are related is generated from the AppHost project using a special publisher:

```sh
dotnet run --publisher manifest --output-path manifest.json
```

### Initializing the AZD Project

Once you have obtained the manifest, authenticate into Azure and Initialise the deployment to your chose subscription:

```sh
azd auth login

azd init -s <YOUR SUBSCRIPTION ID>
```

### Provisioning and Deploying the project

Once the deployment is initialize, you are ready to deploy:

```sh
azd up
```

DONE!
