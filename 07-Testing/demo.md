# Testing

Demo of new .NET 8 testing features

## Pre-requisites

- .NET 8



# Simplified Test Project Configuration in .NET 8 and Streamlined Logging Integration

Built-in Configuration and Dependency Injection Improvements:

.NET 8 continues to enhance the built-in configuration and DI mechanisms, making them more intuitive and easier to use.
Integration of TimeProvider and other new abstractions simplify common patterns like time management in tests.

Enhanced Environment Configuration:

Better support for environment-specific configurations, which is crucial for running tests in different environments (e.g., development, staging, production).
Improved support for environment variable overrides and configuration providers.

Streamlined Logging Integration:

Enhanced logging setup with more straightforward configuration using the built-in logging providers.
Improved logging output formatting and capabilities, making it easier to capture and understand log data during test runs.

Unified Dependency Injection Experience:

Continued improvements in the DI system, making it easier to register and resolve dependencies, including in test scenarios.
Better integration with test frameworks like xUnit, NUnit, and MSTest, making it simpler to set up and use DI in tests.

## Setup 

To create the project from scratch:

```bash
dotnet new xunit -o SimplifiedConfigTestProject
cd SimplifiedConfigTestProject
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


# TimeProvider

TimeProvider is an abstract base class introduced in .NET 8 that provides a standard way to access time-related information.

Key Features:

**Standardization**: TimeProvider provides a unified approach to accessing time-related information, reducing the need for various custom implementations scattered across the codebase.
**Testability**: By using a TimeProvider, time-dependent code can be easily tested. Developers can inject mock or custom implementations of TimeProvider to simulate different times and durations in unit tests.
**Flexibility**: TimeProvider can be extended to create custom time providers that suit specific needs, such as simulating time in different time zones or providing consistent time sources in distributed systems.
**Separation of Concerns**: It separates the concerns of obtaining the current time from business logic, making the code cleaner and more maintainable.

Explain why using DateTime in testing is a big task:

1. Create ITimeService interface
2. Implementation of it, say SystemTimeInterface
3. Create a TimeService using implementation
4. Create a Mock of the Time Service
5. Use the Mock in your tests

Now, show how simple it is with TimeProvider in .NET 9 and using the FaketimeProvider

```bash
dotnet new xunit -o TimeProviderTestProject
cd TimeProviderTestProject
```

```bash
dotnet add package xunit
dotnet add package xunit.runner.visualstudio
dotnet add package Microsoft.Extensions.TimeProvider.Testing
```

Create a Services\TimeService.cs class and copy over the following code

```csharp
public class TimeService
{
    private readonly TimeProvider _timeProvider;

    public TimeService(TimeProvider timeProvider)
    {
        _timeProvider = timeProvider;
    }

    public DateTime GetCurrentTime()
    {
        return _timeProvider.GetUtcNow().DateTime;
    }

    public DateOnly GetCurrentDate()
    {
        return DateOnly.FromDateTime(_timeProvider.GetUtcNow().DateTime);
    }
}
```

Explain how we did step 1-3 in one

Next, create a test using FakeTimeProvider

Create class Tests\TimeServiceTests.cs and copy over the following code:

```csharp 
public class TimeServiceTests
{
    [Fact]
    public void GetCurrentTime_ReturnsMockedTime()
    {
        // Arrange
        var fakeTimeProvider = new FakeTimeProvider(new DateTimeOffset(new DateTime(2023, 1, 1)));
        var timeService = new TimeService(fakeTimeProvider);

        // Act
        var currentTime = timeService.GetCurrentTime();

        // Assert
        Assert.Equal(new DateTime(2023, 1, 1), currentTime);
    }

    [Fact]
    public void GetCurrentDate_ReturnsMockedDate()
    {
        // Arrange
        var fakeTimeProvider = new FakeTimeProvider(new DateTimeOffset(new DateTime(2023, 1, 1)));
        var timeService = new TimeService(fakeTimeProvider);

        // Act
        var currentDate = timeService.GetCurrentDate();

        // Assert
        Assert.Equal(new DateOnly(2023, 1, 1), currentDate);
    }
}
```

Show how simple it is and that we did the last 2 steps in one, which results in 2 steps to use time for testing.