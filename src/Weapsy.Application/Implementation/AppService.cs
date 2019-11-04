using System;
using System.Threading.Tasks;
using Weapsy.Cqrs;
using Weapsy.Domain.Apps;
using Weapsy.Domain.Apps.Commands;
using Weapsy.Domain.Apps.Rules;
using Weapsy.Reporting.Apps;
using Weapsy.Reporting.Apps.Queries;

namespace Weapsy.Application.Implementation
{
    public class AppService : IAppService
    {
        private readonly IDispatcher _dispatcher;
        private readonly IAppRules _appRules;

        public AppService(IDispatcher dispatcher, IAppRules appRules)
        {
            _dispatcher = dispatcher;
            _appRules = appRules;
        }

        public async Task CreateAppAsync(CreateApp command)
        {
            await _dispatcher.SendAndPublishAsync<CreateApp, App>(command);
        }

        public async Task UpdateAppDetailsAsync(UpdateAppDetails command)
        {
            await _dispatcher.SendAndPublishAsync<UpdateAppDetails, App>(command);
        }

        public bool IsAppNameUnique(string name)
        {
            return _appRules.IsAppNameUnique(name);
        }

        public async Task<AppAdminModel> GetAppAdminModelAsync(GetAppAdminModel query)
        {
            return await _dispatcher.GetResultAsync<GetAppAdminModel, AppAdminModel>(query);
        }

        public async Task<bool> IsAppInstalledAsync(string name)
        {
            return await _dispatcher.GetResultAsync<IsAppInstalled, bool>(new IsAppInstalled
            {
                Name = name
            });
        }
    }
}