using System.Security.Claims;

using ServerCore.Services;

namespace ServerApi.Authentication;

public class UserSession : IUserSession
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private Lazy<Guid> _userId;

    public Guid UserId => _userId.Value;

    public UserSession(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        _userId = new Lazy<Guid>(() =>
        {
            var userIdentity = _httpContextAccessor.HttpContext?.User;
            var userIdClaim = userIdentity?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            return userIdClaim == null ? Guid.Empty : Guid.Parse(userIdClaim.Value);
        });
    }
}