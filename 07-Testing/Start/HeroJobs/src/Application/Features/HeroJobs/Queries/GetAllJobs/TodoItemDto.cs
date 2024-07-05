namespace HeroJobs.Application.Features.HeroJobs.Queries.GetAllJobs;

public class HeroJobDto
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public bool Done { get; set; }
}