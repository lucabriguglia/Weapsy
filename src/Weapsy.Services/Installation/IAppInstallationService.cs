using Weapsy.Core.Domain;

namespace Weapsy.Services.Installation
{
    public interface IAppInstallationService : IService
    {
        void VerifyAppInstallation();
        void InstallDefaultApps();
    }
}
