using System;
using System.Threading.Tasks;
using Weapsy.Infrastructure.Caching;
using Weapsy.Infrastructure.Domain;
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

        public void Handle(LanguageCreated @event)
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
    }
}
