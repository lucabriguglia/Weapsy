using System;
using System.Threading.Tasks;
using Weapsy.Core.Caching;
using Weapsy.Core.Domain;
using Weapsy.Domain.Languages.Events;

namespace Weapsy.Reporting.Data.Default.Languages
{
    public class LanguageEventsHandler : 
        IEventHandler<LanguageCreated>,
        IEventHandler<LanguageDeleted>,
        IEventHandler<LanguageActivated>
    {
        private readonly ICacheManager _cacheManager;

        public LanguageEventsHandler(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }

        public async Task Handle(LanguageCreated @event)
        {
            await ClearCache(@event.SiteId);
        }

        public async Task Handle(LanguageDeleted @event)
        {
            await ClearCache(@event.SiteId);
        }

        public async Task Handle(LanguageActivated @event)
        {
            await ClearCache(@event.SiteId);
        }

        private Task ClearCache(Guid siteId)
        {
            return Task.Run(() => _cacheManager.Remove(string.Format(CacheKeys.LanguagesCacheKey, siteId)));
        }
    }
}
