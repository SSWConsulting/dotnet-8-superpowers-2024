using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Xunit;

namespace SimplifiedTestProject;

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