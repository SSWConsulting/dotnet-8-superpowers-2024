using HeroJobs.Application.Features.HeroJobs.Commands.CreateJob;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http.Json;
using WebApi.IntegrationTests.Common.Fixtures;

namespace WebApi.IntegrationTests.Endpoints.HeroJobs;

public class CreateHeroJobEndpointTests(TestingDatabaseFixture fixture, ITestOutputHelper output)
    : IntegrationTestBase(fixture, output)
{
    [Fact]
    public async Task ShouldCreateHeroJob()
    {
        // Arrange
        var cmd = new CreateHeroJobCommand("Shopping");
        var client = GetAnonymousClient();

        // Act
        var result = await client.PostAsJsonAsync("/HeroJobs", cmd);

        // Assert
        var item = await Context.HeroJobs.FirstOrDefaultAsync(t => t.JobName == cmd.Title);

        result.StatusCode.Should().Be(HttpStatusCode.OK);
        item.Should().NotBeNull();
        item!.JobName.Should().Be(cmd.Title);
        item.CreatedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(10));
    }

    [Fact]
    public async Task Create_WithDuplicateTitle_ShouldFail()
    {
        // Arrange
        var cmd = new CreateHeroJobCommand("Shopping");
        var client = GetAnonymousClient();
        var createHeroJob = async () => await client.PostAsJsonAsync("/HeroJobs", cmd);
        await createHeroJob();

        // Act
        var result = await createHeroJob();
        var validation = await result.Content.ReadFromJsonAsync<ValidationProblemDetails>();

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        validation.Should().NotBeNull();
        validation!.Errors.Should().HaveCount(1);
    }


}