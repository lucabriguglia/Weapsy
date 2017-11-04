using Weapsy.Domain.ModuleTypes.Events;
using Weapsy.Framework.Caching;
using Weapsy.Framework.Events;

namespace Weapsy.Data.Reporting.Apps
{
    public class ModuleTypeEventsHandler : 
        IEventHandler<ModuleTypeCreatedEvent>,
        IEventHandler<ModuleTypeDetailsUpdatedEvent>
    {
        private readonly ICacheManager _cacheManager;

        public ModuleTypeEventsHandler(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }

        public void Handle(ModuleTypeCreatedEvent @event)
        {
            ClearCache();
        }

        public void Handle(ModuleTypeDetailsUpdatedEvent @event)
        {
            ClearCache();
        }

        private void ClearCache()
        {
             _cacheManager.Remove(CacheKeys.ModuleTypesCacheKey);
        }
    }
}
