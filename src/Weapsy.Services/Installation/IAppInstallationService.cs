using System.Collections.Generic;
using Weapsy.Domain.Apps.Commands;
using Weapsy.Domain.ModuleTypes.Commands;
using Weapsy.Framework.Domain;

namespace Weapsy.Services.Installation
{
    public interface IAppInstallationService : IService
    {
        void EnsureAppInstalled(CreateApp createApp, IEnumerable<CreateModuleType> createModuleTypes);
    }
}
