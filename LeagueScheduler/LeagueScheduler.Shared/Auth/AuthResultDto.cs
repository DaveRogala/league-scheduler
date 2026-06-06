using System.Collections.Generic;

namespace LeagueScheduler.Shared.Auth
{
    public record AuthResultDto
    {
        public bool Success { get; init; }
        public string? Token { get; init; }
        public string? Email { get; init; }
        public string? DisplayName { get; init; }
        public List<string> Errors { get; init; } = new();
    }
}
