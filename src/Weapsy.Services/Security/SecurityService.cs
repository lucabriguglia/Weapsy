using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using Weapsy.Framework.Identity;

namespace Weapsy.Services.Security
{
    public class SecurityService : ISecurityService
    {
        public bool IsUserAuthorized(IPrincipal user, IEnumerable<string> roleNames)
        {
            if (user == null || roleNames == null || !roleNames.Any())
                return false;

            foreach (var role in roleNames)
            {
                if (role == Everyone.Name)
                    return true;

                if (role == Registered.Name && user.Identity.IsAuthenticated)
                    return true;

                if (role == Anonymous.Name && !user.Identity.IsAuthenticated)
                    return true;

                if (user.IsInRole(role))
                    return true;
            }

            return false;
        }
    }
}
