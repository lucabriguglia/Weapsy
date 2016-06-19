using System.Threading.Tasks;
using Weapsy.Core.Caching;
using Weapsy.Core.Domain;
using Weapsy.Domain.Model.ModuleTypes.Events;

namespace Weapsy.Reporting.Data.EventHandlers
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
            return Task.Run(() => _cacheManager.Remove(CacheKeys.MODULE_TYPES_CACHE_KEY));
        }
    }
}
