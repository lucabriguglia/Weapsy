using System.Collections.Generic;
using Weapsy.Domain.Apps;
using Weapsy.Domain.ModuleTypes;
using Weapsy.Domain.ModuleTypes.Commands;
using Weapsy.Domain.Apps.Commands;
using Weapsy.Framework.Commands;
using Weapsy.Framework.Queries;
using Weapsy.Reporting.Apps.Queries;

namespace Weapsy.Services.Installation
{
    public class AppInstallationService : IAppInstallationService
    {
        private readonly ICommandSender _commandSender;
        private readonly IQueryDispatcher _queryDispatcher;

        public AppInstallationService(ICommandSender commandSender, 
            IQueryDispatcher queryDispatcher)
        {
            _commandSender = commandSender;
            _queryDispatcher = queryDispatcher;
        }

        public void EnsureAppInstalled(CreateApp createApp, IEnumerable<CreateModuleType> createModuleTypes)
        {
            if (_queryDispatcher.Dispatch<IsAppInstalled, bool>(new IsAppInstalled { Name = createApp.Name }))
                return;

            _commandSender.Send<CreateApp, App>(createApp, false);

            foreach (var createModuleType in createModuleTypes)
                _commandSender.Send<CreateModuleType, ModuleType>(createModuleType, false);
        }
    }
}
