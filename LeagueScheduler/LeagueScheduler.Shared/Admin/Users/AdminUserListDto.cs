namespace LeagueScheduler.Shared.Admin.Users;

public record AdminUserListDto(
    IReadOnlyList<AdminUserDto> Users,
    int TotalCount,
    int Page,
    int PageSize
);
