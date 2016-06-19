using Weapsy.Core.Domain;

namespace Weapsy.Domain.Services.Installation
{
    public interface ISiteInstallationService : IService
    {
        void InstallDefaultSite();
    }
}
