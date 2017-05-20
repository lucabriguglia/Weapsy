using System;
using System.Threading.Tasks;
using Weapsy.Domain.Languages.Events;
using Weapsy.Framework.Caching;
using Weapsy.Framework.Events;

namespace Weapsy.Data.Reporting.Languages
{
    public class LanguageEventsHandlers : 
        IEventHandlerAsync<LanguageCreated>,
        IEventHandler<LanguageDetailsUpdated>,
        IEventHandler<LanguageDeleted>,
        IEventHandler<LanguageActivated>,
        IEventHandler<LanguageReordered>
    {
        private readonly ICacheManager _cacheManager;

        public LanguageEventsHandlers(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }

        public async Task HandleAsync(LanguageCreated @event)
        {
            await ClearCacheAsync(@event.SiteId);
        }

        public void Handle(LanguageDetailsUpdated @event)
        {
            ClearCache(@event.SiteId);
        }

        public void Handle(LanguageDeleted @event)
        {
            ClearCache(@event.SiteId);
        }

        public void Handle(LanguageActivated @event)
        {
            ClearCache(@event.SiteId);
        }

        public void Handle(LanguageReordered @event)
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
