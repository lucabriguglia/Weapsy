using System.Threading.Tasks;
using Weapsy.Infrastructure.Caching;
using Weapsy.Infrastructure.Domain;
using Weapsy.Domain.ModuleTypes.Events;

namespace Weapsy.Reporting.Data.Default.ModuleTypes
{
    public class ModuleTypeEventsHandler : 
        IEventHandler<ModuleTypeCreated>,
        IEventHandler<ModuleTypeDetailsUpdated>
    {
        private readonly ICacheManager _cacheManager;

        public ModuleTypeEventsHandler(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }

        public async Task Handle(ModuleTypeCreated @event)
        {
            await ClearCache();
        }

        public async Task Handle(ModuleTypeDetailsUpdated @event)
        {
            await ClearCache();
        }

        private Task ClearCache()
        {
            return Task.Run(() => _cacheManager.Remove(CacheKeys.ModuleTypesCacheKey));
        }
    }
}
