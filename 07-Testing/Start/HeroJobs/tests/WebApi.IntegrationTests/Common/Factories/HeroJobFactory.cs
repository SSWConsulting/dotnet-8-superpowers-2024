using Bogus;
using HeroJobs.Domain.HeroJobs;

namespace WebApi.IntegrationTests.Common.Factories;

public static class HeroJobFactory
{
    private static readonly Faker<HeroJob> Faker = new Faker<HeroJob>().CustomInstantiator(f => HeroJob.Create(
        f.Lorem.Sentence()
    ));

    public static HeroJob Generate() => Faker.Generate();

    public static IEnumerable<HeroJob> Generate(int num) => Faker.Generate(num);
}