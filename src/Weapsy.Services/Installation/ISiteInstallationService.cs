using System.Threading.Tasks;
using Weapsy.Infrastructure.Domain;

namespace Weapsy.Services.Installation
{
    public interface ISiteInstallationService : IService
    {
        void VerifySiteInstallation();
        void InstallDefaultSite();
        Task EnsureSiteInstalled(string name);
    }
}
