using System;
using System.Threading.Tasks;
using Weapsy.Domain.Languages.Events;
using Weapsy.Infrastructure.Caching;
using Weapsy.Infrastructure.Domain;

namespace Weapsy.Reporting.Data.Languages
{
    public class LanguageEventsHandler : 
        IEventHandlerAsync<LanguageCreated>,
        IEventHandlerAsync<LanguageDetailsUpdated>,
        IEventHandlerAsync<LanguageDeleted>,
        IEventHandlerAsync<LanguageActivated>
    {
        private readonly ICacheManager _cacheManager;

        public LanguageEventsHandler(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }

        public Task HandleAsync(LanguageCreated @event)
        {
            return ClearCache(@event.SiteId);
        }

        public async Task HandleAsync(LanguageDetailsUpdated @event)
        {
            await ClearCache(@event.SiteId);
        }

        public async Task HandleAsync(LanguageDeleted @event)
        {
            await ClearCache(@event.SiteId);
        }

        public async Task HandleAsync(LanguageActivated @event)
        {
            await ClearCache(@event.SiteId);
        }

        private Task ClearCache(Guid siteId)
        {
            return Task.Run(() => _cacheManager.Remove(string.Format(CacheKeys.LanguagesCacheKey, siteId)));
        }
    }
}
