using System.ComponentModel;

using Spectre.Console;
using Spectre.Console.Cli;

namespace HeroFinder.Console.Heroes.CreateHero;

/// <summary>
/// CLI parameters needed for the 'heroes create' command
/// </summary>
public class CreateHeroSettings : CommandSettings
{
    [Description("The name of the hero.")]
    [CommandArgument(0, "[name]")]
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public required string Name { get; init; }

    [Description("The alias (real name) of the hero.")]
    [CommandArgument(1, "[alias]")]
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public required string Alias { get; init; }

    [Description("Super power of the hero")]
    [CommandOption("-p|--power <POWERS>")]
    public string[]? Powers { get; init; }

    /// <summary>
    /// Validate required parameters
    /// </summary>
    public override ValidationResult Validate()
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            return ValidationResult.Error("Name is required");
        }

        if (string.IsNullOrWhiteSpace(Alias))
        {
            return ValidationResult.Error("Alias is required");
        }

        return ValidationResult.Success();
    }
}