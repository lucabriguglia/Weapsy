using System.Threading.Tasks;
using Weapsy.Infrastructure.Caching;
using Weapsy.Infrastructure.Domain;
using Weapsy.Domain.ModuleTypes.Events;

namespace Weapsy.Reporting.Data.Default.ModuleTypes
{
    public class ModuleTypeEventsHandler : 
        IEventHandlerAsync<ModuleTypeCreated>,
        IEventHandlerAsync<ModuleTypeDetailsUpdated>
    {
        private readonly ICacheManager _cacheManager;

        public ModuleTypeEventsHandler(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }

        public async Task HandleAsync(ModuleTypeCreated @event)
        {
            await ClearCache();
        }

        public async Task HandleAsync(ModuleTypeDetailsUpdated @event)
        {
            await ClearCache();
        }

        private Task ClearCache()
        {
            return Task.Run(() => _cacheManager.Remove(CacheKeys.ModuleTypesCacheKey));
        }
    }
}
