using System.Collections.Generic;
using System.Security.Principal;

namespace Weapsy.Services.Security
{
    public interface ISecurityService
    {
        bool IsUserAuthorized(IPrincipal user, IEnumerable<string> roleNames);
    }
}
