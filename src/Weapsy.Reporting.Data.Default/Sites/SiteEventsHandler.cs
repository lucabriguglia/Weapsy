using System;
using Weapsy.Infrastructure.Caching;
using Weapsy.Infrastructure.Domain;
using Weapsy.Domain.Sites.Events;
using Weapsy.Reporting.Languages;

namespace Weapsy.Reporting.Data.Default.Sites
{
    public class SiteEventsHandler : 
        IEventHandler<SiteCreated>,
        IEventHandler<SiteDetailsUpdated>
    {
        private readonly ICacheManager _cacheManager;
        private readonly ILanguageFacade _languageFacade;

        public SiteEventsHandler(ICacheManager cacheManager, 
            ILanguageFacade languageFacade)
        {
            _cacheManager = cacheManager;
            _languageFacade = languageFacade;
        }

        public void Handle(SiteCreated @event)
        {
            ClearCache(@event.AggregateRootId, @event.Name);
        }

        public void Handle(SiteDetailsUpdated @event)
        {
            ClearCache(@event.AggregateRootId, @event.Name);
        }

        private void ClearCache(Guid siteId, string name)
        {
            foreach (var language in _languageFacade.GetAllActive(siteId))
                _cacheManager.Remove(string.Format(CacheKeys.SiteInfoCacheKey, name, language.Id));

            _cacheManager.Remove(string.Format(CacheKeys.SiteInfoCacheKey, name, Guid.Empty));
        }
    }
}
