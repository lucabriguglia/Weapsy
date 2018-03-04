using System.Collections.Generic;
using Weapsy.Domain.Apps.Commands;
using Weapsy.Domain.ModuleTypes.Commands;

namespace Weapsy.Services.Installation
{
    public interface IAppInstallationService
    {
        void EnsureAppInstalled(CreateApp createApp, IEnumerable<CreateModuleType> createModuleTypes);
    }
}
