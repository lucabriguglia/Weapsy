using System;
using System.Threading.Tasks;
using Weapsy.Domain.Pages.Events;
using Weapsy.Infrastructure.Caching;
using Weapsy.Infrastructure.Domain;
using Weapsy.Reporting.Languages;

namespace Weapsy.Reporting.Data.Pages
{
    public class PageEventsHandler : 
        IEventHandlerAsync<PageCreated>,
        IEventHandlerAsync<PageDeleted>,
        IEventHandlerAsync<PageActivated>,
        IEventHandlerAsync<PageDetailsUpdated>,
        IEventHandlerAsync<PageHidden>,
        IEventHandlerAsync<PageModuleAdded>,
        IEventHandlerAsync<PageModuleDetailsUpdated>,
        IEventHandlerAsync<PageModulesReordered>,
        IEventHandlerAsync<PageModuleRemoved>
    {
        private readonly ICacheManager _cacheManager;
        private readonly ILanguageFacade _languageFacade;

        public PageEventsHandler(ICacheManager cacheManager, 
            ILanguageFacade languageFacade)
        {
            _cacheManager = cacheManager;
            _languageFacade = languageFacade;
        }

        public async Task HandleAsync(PageCreated @event)
        {
        }

        public Task HandleAsync(PageDetailsUpdated @event)
        {
            return ClearPageCache(@event.SiteId, @event.AggregateRootId);
        }

        public async Task HandleAsync(PageDeleted @event)
        {
            await ClearPageCache(@event.SiteId, @event.AggregateRootId);
        }

        public async Task HandleAsync(PageActivated @event)
        {
            await ClearPageCache(@event.SiteId, @event.AggregateRootId);
            await ClearMenuCache(@event.SiteId);
        }

        public async Task HandleAsync(PageHidden @event)
        {
            await ClearPageCache(@event.SiteId, @event.AggregateRootId);
            await ClearMenuCache(@event.SiteId);
        }

        public async Task HandleAsync(PageModuleAdded @event)
        {
            await ClearPageCache(@event.SiteId, @event.AggregateRootId);
        }

        public async Task HandleAsync(PageModuleDetailsUpdated @event)
        {
            await ClearPageCache(@event.SiteId, @event.AggregateRootId);
        }

        public async Task HandleAsync(PageModulesReordered @event)
        {
            await ClearPageCache(@event.SiteId, @event.AggregateRootId);
        }

        public async Task HandleAsync(PageModuleRemoved @event)
        {
            await ClearPageCache(@event.SiteId, @event.AggregateRootId);
        }

        private Task ClearPageCache(Guid siteId, Guid pageId)
        {
            return Task.Run(() =>
            {
                foreach (var language in _languageFacade.GetAllActive(siteId).Result)
                    _cacheManager.Remove(string.Format(CacheKeys.PageInfoCacheKey, siteId, pageId, language.Id));

                _cacheManager.Remove(string.Format(CacheKeys.PageInfoCacheKey, siteId, pageId, Guid.Empty));
            });
        }

        private Task ClearMenuCache(Guid siteId)
        {
            return Task.Run(() =>
            {
                foreach (var language in _languageFacade.GetAllActive(siteId).Result)
                    _cacheManager.Remove(string.Format(CacheKeys.MenuCacheKey, siteId, "Main", language.Id));

                _cacheManager.Remove(string.Format(CacheKeys.MenuCacheKey, siteId, "Main"));
            });
        }
    }
}
