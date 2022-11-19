global using SeerbitHackaton.Core.AspnetCore.Identity;

namespace SeerbitHackaton.Core.AspnetCore
{
    public class HttpUserService : IHttpUserService
    {
        private readonly IHttpContextAccessor _httpContext;

        public HttpUserService(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }

        public UserPrincipal GetCurrentUser()
        {
            if (_httpContext.HttpContext != null && _httpContext.HttpContext.User != null)
            {
                return new UserPrincipal(_httpContext.HttpContext.User);
            }

            throw new Exception("Current user cannot be determined");
        }

        //public bool CheckCurrentUserHasPermission(Permission permission)
        //{
        //    var user = GetCurrentUser();
        //    var checkUserHasPermission = user.Claims
        //       .Where(x => x.Type.Equals(nameof(Permission)))
        //       .Any(x => x.Value == permission.ToString());

        //    return checkUserHasPermission;
        //}
    }

    public interface IHttpUserService
    {
        UserPrincipal GetCurrentUser();

        //bool CheckCurrentUserHasPermission(Permission permission);
    }
}