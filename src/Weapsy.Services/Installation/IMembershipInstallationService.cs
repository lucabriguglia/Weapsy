using Weapsy.Infrastructure.Domain;

namespace Weapsy.Services.Installation
{
    public interface IMembershipInstallationService : IService
    {
        void VerifyUserCreation();
    }
}
