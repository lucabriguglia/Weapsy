using System;
using System.Threading.Tasks;
using Weapsy.Domain.Languages.Events;
using Weapsy.Framework.Caching;
using Weapsy.Framework.Events;

namespace Weapsy.Data.Reporting.Languages
{
    public class LanguageEventsHandlers : 
        IEventHandlerAsync<LanguageCreatedEvent>,
        IEventHandler<LanguageDetailsUpdatedEvent>,
        IEventHandler<LanguageDeletedEvent>,
        IEventHandler<LanguageActivatedEvent>,
        IEventHandler<LanguageReorderedEvent>
    {
        private readonly ICacheManager _cacheManager;

        public LanguageEventsHandlers(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }

        public async Task HandleAsync(LanguageCreatedEvent @event)
        {
            await ClearCacheAsync(@event.SiteId);
        }

        public void Handle(LanguageDetailsUpdatedEvent @event)
        {
            ClearCache(@event.SiteId);
        }

        public void Handle(LanguageDeletedEvent @event)
        {
            ClearCache(@event.SiteId);
        }

        public void Handle(LanguageActivatedEvent @event)
        {
            ClearCache(@event.SiteId);
        }

        public void Handle(LanguageReorderedEvent @event)
        {
            ClearCache(@event.SiteId);
        }

        private void ClearCache(Guid siteId)
        {
            _cacheManager.Remove(string.Format(CacheKeys.LanguagesCacheKey, siteId));
        }

        private async Task ClearCacheAsync(Guid siteId)
        {
            await _cacheManager.RemoveAsync(string.Format(CacheKeys.LanguagesCacheKey, siteId));
        }
    }
}
