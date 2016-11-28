using System;
using System.Threading.Tasks;
using Weapsy.Infrastructure.Caching;
using Weapsy.Infrastructure.Domain;
using Weapsy.Domain.Sites.Events;
using Weapsy.Reporting.Languages;

namespace Weapsy.Reporting.Data.Default.Sites
{
    public class SiteEventsHandler : 
        IEventHandlerAsync<SiteCreated>,
        IEventHandlerAsync<SiteDetailsUpdated>
    {
        private readonly ICacheManager _cacheManager;
        private readonly ILanguageFacade _languageFacade;

        public SiteEventsHandler(ICacheManager cacheManager, 
            ILanguageFacade languageFacade)
        {
            _cacheManager = cacheManager;
            _languageFacade = languageFacade;
        }

        public async Task HandleAsync(SiteCreated @event)
        {
            await ClearCache(@event.AggregateRootId, @event.Name);
        }

        public async Task HandleAsync(SiteDetailsUpdated @event)
        {
            await ClearCache(@event.AggregateRootId, @event.Name);
        }

        private Task ClearCache(Guid siteId, string name)
        {
            return Task.Run(() =>
            {
                foreach (var language in _languageFacade.GetAllActive(siteId))
                    _cacheManager.Remove(string.Format(CacheKeys.SiteInfoCacheKey, name, language.Id));

                _cacheManager.Remove(string.Format(CacheKeys.SiteInfoCacheKey, name, Guid.Empty));
            });
        }
    }
}
