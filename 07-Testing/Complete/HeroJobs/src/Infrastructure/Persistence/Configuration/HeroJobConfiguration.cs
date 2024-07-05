using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HeroJobs.Domain.HeroJobs;

namespace HeroJobs.Infrastructure.Persistence.Configuration;

public class HeroJobConfiguration : IEntityTypeConfiguration<HeroJob>
{
    // TODO: Rip out the common pieces that are from BaseEntity (https://github.com/SSWConsulting/HeroJobs/issues/78)
    // virtual method, override 
    // Good marker to enforce that all entities have configuration defined via arch tests
    public void Configure(EntityTypeBuilder<HeroJob> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .HasConversion(x => x.Value,
                x => new HeroJobId(x))
            .ValueGeneratedOnAdd();

        builder.Property(t => t.JobName)
            .HasMaxLength(200)
            .IsRequired();
    }
}