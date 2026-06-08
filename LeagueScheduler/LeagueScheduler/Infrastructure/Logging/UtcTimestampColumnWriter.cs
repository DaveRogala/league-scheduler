using NpgsqlTypes;
using Serilog.Events;
using Serilog.Sinks.PostgreSQL;

namespace LeagueScheduler.Infrastructure.Logging;

// Serilog.Sinks.PostgreSQL 2.3.0 passes logEvent.Timestamp directly to Npgsql.
// Npgsql 7+ requires DateTimeOffset.Offset == 0 (UTC) for timestamptz writes.
// This writer converts to UTC before passing the value.
internal sealed class UtcTimestampColumnWriter : ColumnWriterBase
{
    public UtcTimestampColumnWriter() : base(NpgsqlDbType.TimestampTz) { }

    public override object GetValue(LogEvent logEvent, IFormatProvider? formatProvider = null)
        => logEvent.Timestamp.ToUniversalTime();
}
