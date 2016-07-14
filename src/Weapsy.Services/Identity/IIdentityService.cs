using System.Threading.Tasks;

namespace Weapsy.Services.Identity
{
    public interface IIdentityService
    {
        Task CreateRole(string name);
    }
}
