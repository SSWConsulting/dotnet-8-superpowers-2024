using HeroJobs.Application.Features.HeroJobs.Queries.GetAllJobs;
using System.Net.Http.Json;
using WebApi.IntegrationTests.Common.Factories;
using WebApi.IntegrationTests.Common.Fixtures;

namespace WebApi.IntegrationTests.Endpoints.HeroJobs;

public class GetAllHerosEndpointTests(TestingDatabaseFixture fixture, ITestOutputHelper output)
    : IntegrationTestBase(fixture, output)
{
    [Fact]
    public async Task Should_Return_All_HeroJobs()
    {
        // Arrange
        const int entityCount = 10;
        var entities = HeroJobFactory.Generate(entityCount);
        await AddEntitiesAsync(entities);
        var client = GetAnonymousClient();

        // Act
        var result = await client.GetFromJsonAsync<HeroJobDto[]>("/HeroJobs");

        // Assert
        result.Should().NotBeNull();
        result!.Length.Should().Be(entityCount);
    }
}