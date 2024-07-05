using System.ComponentModel.DataAnnotations;

namespace JusticeLeague.Client;

public record ApiConfig
{
    public const string SectionName = "Api";

    [Required]
    public required string Url { get; init; }

    [Required]
    public required string UserName { get; init; }

    [Required]
    public required string Password { get; init; }
}