using Weapsy.Core.Domain;

namespace Weapsy.Services.Installation
{
    public interface ISiteInstallationService : IService
    {
        void InstallDefaultSite();
    }
}
