using LeagueScheduler.Infrastructure.Audit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeagueScheduler.Infrastructure.Data.Configurations;

public abstract class AuditableEntityConfiguration<T> : IEntityTypeConfiguration<T>
    where T : AuditableEntity
{
    public void Configure(EntityTypeBuilder<T> e)
    {
        e.HasQueryFilter(x => x.DeletedAt == null);
        ConfigureEntity(e);
    }

    protected abstract void ConfigureEntity(EntityTypeBuilder<T> e);
}
