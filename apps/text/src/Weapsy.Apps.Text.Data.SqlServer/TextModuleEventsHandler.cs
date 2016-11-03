using System;
using System.Threading.Tasks;
using Weapsy.Apps.Text.Domain.Events;
using Weapsy.Infrastructure.Caching;
using Weapsy.Infrastructure.Domain;

namespace Weapsy.Apps.Text.Data.SqlServer
{
    public class TextModuleEventsHandler : 
        IEventHandler<TextModuleCreated>,
        IEventHandler<VersionAdded>       
    {
        private readonly ICacheManager _cacheManager;

        public TextModuleEventsHandler(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }

        public Task Handle(TextModuleCreated @event)
        {
            return ClearCache(@event.ModuleId);
        }

        public Task Handle(VersionAdded @event)
        {
            return ClearCache(@event.ModuleId);
        }

        private Task ClearCache(Guid moduleId)
        {
            return Task.Run(() => _cacheManager.Remove(string.Format(CacheKeys.TextModuleCacheKey, moduleId)));
        }
    }
}
