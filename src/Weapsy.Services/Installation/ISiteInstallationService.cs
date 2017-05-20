using System.Threading.Tasks;
using Weapsy.Framework.Domain;

namespace Weapsy.Services.Installation
{
    public interface ISiteInstallationService : IService
    {
        void VerifySiteInstallation();
        void InstallDefaultSite();
        Task EnsureSiteInstalled(string name);
    }
}
