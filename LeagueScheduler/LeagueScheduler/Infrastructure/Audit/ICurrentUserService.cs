namespace LeagueScheduler.Infrastructure.Audit;

public interface ICurrentUserService
{
    Guid? UserId { get; }
}
