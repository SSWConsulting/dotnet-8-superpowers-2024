# Testing

Demo of new .NET 8 testing features

## Pre-requisites

- .NET 8

## Setup 

To create the project from scratch:

```bash
dotnet new xunit -o SimplifiedTestProject
cd SimplifiedTestProject
```


Run the following to install test packages 

```bash
dotnet add package xunit
dotnet add package xunit.runner.visualstudio
dotnet add package Microsoft.Extensions.Configuration
dotnet add package Microsoft.Extensions.Configuration.Json
dotnet add package Microsoft.Extensions.DependencyInjection
dotnet add package Microsoft.Extensions.Logging
dotnet add package Microsoft.Extensions.Options.ConfigurationExtensions
dotnet add package Microsoft.Extensions.Logging.Console
```

Delete the default unit test



## Configure Environment-Specific Settings

Create appsettings.json, appsettings.Development.json, and appsettings.Production.json files in your test project directory.

Copy the following into each of the respective files:

appsettings.json
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning"
    }
  },
  "AppSettings": {
    "ApiBaseUrl": "https://api.example.com"
  }
}
```

appsettings.Development.json
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "Microsoft": "Information"
    }
  },
  "AppSettings": {
    "ApiBaseUrl": "https://dev.api.example.com"
  }
}
```

appsettings.Production.json
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Error",
      "Microsoft": "Critical"
    }
  },
  "AppSettings": {
    "ApiBaseUrl": "https://prod.api.example.com"
  }
}
```

Remember to set each file to be written to the output directory


## Use Configuration Files in Tests

Create a TestBase.cs file in the test project. 


TestBase.cs
```csharp

public class TestBase
{
    protected readonly IConfiguration Configuration;
    protected readonly IServiceProvider ServiceProvider;
    protected readonly ILogger Logger;

    public TestBase()
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

        var configurationBuilder = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true);

        Configuration = configurationBuilder.Build();

        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);

        ServiceProvider = serviceCollection.BuildServiceProvider();
        Logger = ServiceProvider.GetRequiredService<ILogger<TestBase>>();
    }

    private void ConfigureServices(IServiceCollection services)
    {
        services.AddLogging(configure => configure.AddConsole())
                .Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Information);

        // Register other services and configurations
        services.AddSingleton(Configuration);
        services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
    }
}

public class AppSettings
{
    public string ApiBaseUrl { get; set; }
}

```

Explain what is happening in the file

## Write a Test Using the Configuration
Create a ExampleTests.cs file in the test project

```csharp

public class ExampleTests : TestBase
{
    private readonly IOptions<AppSettings> _appSettings;

    public ExampleTests()
    {
        _appSettings = ServiceProvider.GetService<IOptions<AppSettings>>();
    }

    [Fact]
    public void Test_ApiBaseUrl_IsConfiguredCorrectly()
    {
        var apiBaseUrl = _appSettings.Value.ApiBaseUrl;

        Logger.LogInformation("ApiBaseUrl: {ApiBaseUrl}", apiBaseUrl);

        // Assert
        Assert.NotNull(apiBaseUrl);
        Assert.Contains("api.example.com", apiBaseUrl);
    }
}

```

Debug the test in console. 

First test showing test using Development appsettings

```bash
export ASPNETCORE_ENVIRONMENT=Development
dotnet test
```

Second test showing test using Production appsettings

```bash
export ASPNETCORE_ENVIRONMENT=Production
dotnet test
```