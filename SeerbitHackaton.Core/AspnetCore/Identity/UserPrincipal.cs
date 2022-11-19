namespace SeerbitHackaton.Core.AspnetCore.Identity
{
    public class UserPrincipal : ClaimsPrincipal
    {
        public UserPrincipal(ClaimsPrincipal principal) : base(principal)
        {
        }

        private string GetClaimValue(string key)
        {
            var identity = Identity as ClaimsIdentity;
            if (identity == null)
                return null;

            var claim = identity.Claims.FirstOrDefault(c => c.Type == key);
            return claim?.Value;
        }

        public string UserName
        {
            get
            {
                if (FindFirst(JwtClaimTypes.Id) == null)
                    return string.Empty;

                return GetClaimValue(JwtClaimTypes.Id);
            }
        }

        public string Email
        {
            get
            {
                if (FindFirst(JwtClaimTypes.Email) == null)
                    return string.Empty;

                return GetClaimValue(JwtClaimTypes.Email);
            }
        }

        public int UserId
        {
            get
            {
                if (FindFirst(JwtClaimTypes.Subject) == null)
                    return default;

                return Convert.ToInt32(GetClaimValue(JwtClaimTypes.Subject));
            }
        }

        public string FirstName
        {
            get
            {
                var usernameClaim = FindFirst(JwtClaimTypes.GivenName);

                if (usernameClaim == null)
                    return string.Empty;

                return usernameClaim.Value;
            }
        }

        public string LastName
        {
            get
            {
                var usernameClaim = FindFirst(JwtClaimTypes.FamilyName);

                if (usernameClaim == null)
                    return string.Empty;

                return usernameClaim.Value;
            }
        }

        public string TenantId
        {
            get
            {
                var tenantIdClaim = FindFirst(CoreConstants.ClaimsKey.TenantId);
                if (tenantIdClaim is null)
                    return string.Empty;
                return tenantIdClaim.Value;
            }
        }
    }
}