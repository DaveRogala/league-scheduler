using LeagueScheduler.Features.Pronouns.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeagueScheduler.Infrastructure.Data.Configurations;

public class SupportedPronounsConfiguration : IEntityTypeConfiguration<SupportedPronouns>
{
    // Fixed IDs for the built-in sets that cannot be deleted.
    // Referenced in the data migration to map legacy enum values.
    public static readonly Guid HeHimHisId      = new("3fa85f64-5717-4562-b3fc-2c963f66afa1");
    public static readonly Guid SheHerHersId    = new("3fa85f64-5717-4562-b3fc-2c963f66afa2");
    public static readonly Guid TheyThemTheirsId = new("3fa85f64-5717-4562-b3fc-2c963f66afa3");

    public void Configure(EntityTypeBuilder<SupportedPronouns> e)
    {
        e.HasKey(p => p.Id);
        e.Property(p => p.Id).HasDefaultValueSql("uuid_generate_v1mc()");
        e.Property(p => p.Label).IsRequired().HasMaxLength(100);

        e.HasData(
            new SupportedPronouns { Id = HeHimHisId,       Label = "He / Him / His",       IsBuiltIn = true, SortOrder = 1 },
            new SupportedPronouns { Id = SheHerHersId,     Label = "She / Her / Hers",     IsBuiltIn = true, SortOrder = 2 },
            new SupportedPronouns { Id = TheyThemTheirsId, Label = "They / Them / Theirs", IsBuiltIn = true, SortOrder = 3 }
        );
    }
}
