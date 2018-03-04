using System.Threading.Tasks;

namespace Weapsy.Services.Installation
{
    public interface ISiteInstallationService
    {
        void VerifySiteInstallation();
        void InstallDefaultSite();
        Task EnsureSiteInstalled(string name);
    }
}
