using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace SimplifiedTestProject;

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