namespace LeagueScheduler.Shared.Admin.Users;

public record AdminUserDto(
    Guid Id,
    string Email,
    string DisplayName,
    bool IsDisabled,
    IReadOnlyList<string> Roles
);
