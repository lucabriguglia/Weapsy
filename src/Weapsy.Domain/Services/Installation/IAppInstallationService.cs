using Weapsy.Core.Domain;

namespace Weapsy.Domain.Services.Installation
{
    public interface IAppInstallationService : IService
    {
        void InstallDefaultApps();
    }
}
