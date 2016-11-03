using Weapsy.Infrastructure.Domain;

namespace Weapsy.Services.Installation
{
    public interface IAppInstallationService : IService
    {
        void VerifyAppInstallation();
        void InstallDefaultApps();
    }
}
