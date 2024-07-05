namespace HeroFinder.Console.Heroes;

/// <summary>
/// Test data
/// </summary>
public static class HeroFactory
{
    public static List<Hero> CreateHeroes()
    {
        var heroes = new List<Hero>
        {
            new()
            {
                Name = "Superman",
                Alias = "Clark Kent",
                IsBusy = true,
                Affiliation = AffiliationFactory.JusticeLeague,
                HeroPowers =
                [
                    PowersFactory.Flight,
                    PowersFactory.SuperStrength,
                    PowersFactory.HeatVision,
                    PowersFactory.XRayVision,
                    PowersFactory.SuperSpeed,
                    PowersFactory.SuperHearing,
                    PowersFactory.Invulnerability
                ],
                SavedTheCityDates =  [
                    new DateOnly(2000, 01, 01),
                    new DateOnly(2000, 02, 02),
                    new DateOnly(2000, 03, 03),
                ]
            },
            new()
            {
                Name = "Batman",
                Alias = "Bruce Wayne",
                IsBusy = true,
                Affiliation = AffiliationFactory.JusticeLeague,
                HeroPowers =
                [
                    PowersFactory.Wealth,
                    PowersFactory.MartialArts,
                    PowersFactory.Intelligence,
                    PowersFactory.Gadgets
                ],
                SecretHideout = new SecretHideout
                {
                    Street = "Bat Cave",
                    City = "Gotham",
                    Country = "USA"
                }
            },
            new()
            {
                Name = "Wonderwoman",
                Alias = "Diana Prince",
                IsBusy = true,
                Affiliation = AffiliationFactory.JusticeLeague,
                HeroPowers = new List<Power>
                {
                    PowersFactory.Flight,
                    PowersFactory.SuperStrength,
                    PowersFactory.SuperSpeed,
                    PowersFactory.Invulnerability

                }
            },
            new()
            {
                Name = "Wolverine",
                Alias = "Logan",
                IsBusy = false,
                Affiliation = AffiliationFactory.XMen,
                HeroPowers = new List<Power>()
                {
                    PowersFactory.Regeneration,
                    PowersFactory.AdamantiumClaws,
                    PowersFactory.SuperStrength,
                    PowersFactory.SuperSpeed,
                    PowersFactory.SuperSenses
                }
            },
            new()
            {
                Name = "Cyclops",
                Alias = "Scott Summers",
                IsBusy = false,
                Affiliation = AffiliationFactory.XMen,
                HeroPowers =
                [
                    PowersFactory.OpticBlast,
                    PowersFactory.SuperStrength,
                    PowersFactory.SuperSpeed,
                    PowersFactory.SuperSenses
                ]
            },
            new()
            {
                Name = "Spiderman",
                Alias = "Peter Parker",
                IsBusy = false,
                Affiliation = AffiliationFactory.XMen,
                HeroPowers =
                [
                    PowersFactory.SuperStrength,
                    PowersFactory.SuperSpeed,
                    PowersFactory.SuperSenses,
                    PowersFactory.Regeneration
                ]
            }

        };

        return heroes;
    }

    private static class AffiliationFactory
    {
        public static Affiliation JusticeLeague { get; } = new() { Name = "Justice League" };
        public static Affiliation XMen { get; } = new() { Name = "X-Men" };
    }

    private static class PowersFactory
    {
        public static Power Flight { get; } = new() { Name = "Flight" };
        public static Power SuperStrength { get; } = new() { Name = "Super Strength" };
        public static Power HeatVision { get; } = new() { Name = "Heat Vision" };
        public static Power XRayVision { get; } = new() { Name = "X-Ray Vision" };
        public static Power SuperSpeed { get; } = new() { Name = "Super Speed" };
        public static Power SuperHearing { get; } = new() { Name = "Super Hearing" };
        public static Power Invulnerability { get; } = new() { Name = "Invulnerability" };
        public static Power Wealth { get; } = new() { Name = "Wealth" };
        public static Power MartialArts { get; } = new() { Name = "Martial Arts" };
        public static Power Intelligence { get; } = new() { Name = "Intelligence" };
        public static Power Gadgets { get; } = new() { Name = "Gadgets" };
        public static Power Regeneration { get; } = new() { Name = "Regeneration" };
        public static Power AdamantiumClaws { get; } = new() { Name = "Adamantium Claws" };
        public static Power SuperSenses { get; } = new() { Name = "Super Senses" };
        public static Power OpticBlast { get; } = new() { Name = "Optic Blast" };
    }
}