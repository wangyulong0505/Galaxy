using Abp.Runtime.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Galaxy.Web.Utils
{
    public static class AbpSessionExtend
    {
        public static string GetUserName(this IAbpSession session)
        {
            return GetClaimValue(ClaimTypes.Name);
        }

        private static string GetClaimValue(string claimType)
        {
            var claimPrincipal = DefaultPrincipalAccessor.Instance.Principal;

            var claim = claimPrincipal?.Claims.FirstOrDefault(c => c.Type == claimType);
            if (string.IsNullOrEmpty(claim?.Value))
            {
                return null;
            }
            return claim.Value;
        }
    }
}
