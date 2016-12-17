using System;
using System.Threading.Tasks;
using Weapsy.Domain.Languages.Events;
using Weapsy.Infrastructure.Caching;
using Weapsy.Infrastructure.Dispatcher;
using Weapsy.Infrastructure.Domain;

namespace Weapsy.Reporting.Data.Languages
{
    public class LanguageEventsHandler : 
        IEventHandlerAsync<LanguageCreated>,
        IEventHandler<LanguageDetailsUpdated>,
        IEventHandler<LanguageDeleted>,
        IEventHandler<LanguageActivated>
    {
        private readonly ICacheManager _cacheManager;

        public LanguageEventsHandler(ICacheManager cacheManager)
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
