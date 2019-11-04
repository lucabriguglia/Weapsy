using System.Collections.Generic;
using Weapsy.Cqrs;
using Weapsy.Domain.Apps;
using Weapsy.Domain.ModuleTypes;
using Weapsy.Domain.ModuleTypes.Commands;
using Weapsy.Domain.Apps.Commands;
using Weapsy.Reporting.Apps.Queries;

namespace Weapsy.Services.Installation
{
    public class AppInstallationService : IAppInstallationService
    {
        private readonly IDispatcher _dispatcher;

        public AppInstallationService(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public void EnsureAppInstalled(CreateApp createApp, IEnumerable<CreateModuleType> createModuleTypes)
        {
            if (_dispatcher.GetResult<IsAppInstalled, bool>(new IsAppInstalled { Name = createApp.Name }))
                return;

            _dispatcher.SendAndPublishAsync<CreateApp, App>(createApp).GetAwaiter().GetResult();

            foreach (var createModuleType in createModuleTypes)
                _dispatcher.SendAndPublish<CreateModuleType, ModuleType>(createModuleType);
        }
    }
}
