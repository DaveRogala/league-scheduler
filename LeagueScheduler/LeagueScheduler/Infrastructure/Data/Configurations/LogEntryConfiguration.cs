using LeagueScheduler.Features.Admin.Logs.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeagueScheduler.Infrastructure.Data.Configurations
{
    public class LogEntryConfiguration : IEntityTypeConfiguration<LogEntry>
    {
        public void Configure(EntityTypeBuilder<LogEntry> e)
        {
            e.ToTable("logs");
            e.HasKey(l => l.Id);
            e.Property(l => l.Id).HasDefaultValueSql("uuid_generate_v1mc()").HasColumnName("id");
            e.Property(l => l.Timestamp).IsRequired().HasColumnName("timestamp");
            e.Property(l => l.Level).IsRequired().HasMaxLength(20).HasColumnName("level");
            e.Property(l => l.Template).IsRequired().HasColumnName("template");
            e.Property(l => l.Message).IsRequired().HasColumnName("message");
            e.Property(l => l.Exception).HasColumnName("exception");
            e.Property(l => l.Properties).HasColumnType("jsonb").HasColumnName("properties");
        }
    }
}
