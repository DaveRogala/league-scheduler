using LeagueScheduler.Features.Admin.Logs.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeagueScheduler.Infrastructure.Data.Configurations
{
    public class LogEntryConfiguration : IEntityTypeConfiguration<LogEntry>
    {
        public void Configure(EntityTypeBuilder<LogEntry> e)
        {
            e.ToTable("Logs");
            e.HasKey(l => l.Id);
            e.Property(l => l.Id).HasDefaultValueSql("uuid_generate_v1mc()");
            e.Property(l => l.Timestamp).IsRequired();
            e.Property(l => l.Level).IsRequired().HasMaxLength(20);
            e.Property(l => l.Template).IsRequired();
            e.Property(l => l.Message).IsRequired();
            e.Property(l => l.Exception);
            e.Property(l => l.Properties).HasColumnType("jsonb");
        }
    }
}
