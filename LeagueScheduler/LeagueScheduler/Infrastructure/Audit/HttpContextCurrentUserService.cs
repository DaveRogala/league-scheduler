using System.Security.Claims;

namespace LeagueScheduler.Infrastructure.Audit;

public class HttpContextCurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _accessor;

    public HttpContextCurrentUserService(IHttpContextAccessor accessor)
        => _accessor = accessor;

    public Guid? UserId
    {
        get
        {
            var claim = _accessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            return claim is not null && Guid.TryParse(claim, out var id) ? id : null;
        }
    }
}
