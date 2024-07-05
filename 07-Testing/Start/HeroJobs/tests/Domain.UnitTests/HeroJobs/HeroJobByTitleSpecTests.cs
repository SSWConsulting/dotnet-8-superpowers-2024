using HeroJobs.Domain.HeroJobs;

namespace HeroJobs.Domain.UnitTests.HeroJobs;

public class HeroJobByTitleSpecTests
{
    private readonly List<HeroJob> _entities =
    [
        HeroJob.Create("Apple"),
        HeroJob.Create("Banana"),
        HeroJob.Create("Apple 2"),
        HeroJob.Create("Banana 2"),
        HeroJob.Create("Hello world 2")
    ];

    [Theory]
    [InlineData("Apple")]
    [InlineData("Banana")]
    [InlineData("Apple 2")]
    [InlineData("Banana 2")]
    public void Evaluate_WithList_ShouldReturnByTitle(string textToSearch)
    {
        var query = new HeroJobByTitleSpec(textToSearch);
        var result = query.Evaluate(_entities).ToList();

        result.Count.Should().Be(1);
        result.First().JobName.Should().Be(textToSearch);
    }
}