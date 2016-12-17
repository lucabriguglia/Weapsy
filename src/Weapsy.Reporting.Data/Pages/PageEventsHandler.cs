using System;
using Weapsy.Domain.Pages.Events;
using Weapsy.Infrastructure.Caching;
using Weapsy.Infrastructure.Dispatcher;
using Weapsy.Infrastructure.Domain;
using Weapsy.Reporting.Languages;

namespace Weapsy.Reporting.Data.Pages
{
    public class PageEventsHandler : 
        IEventHandler<PageCreated>,
        IEventHandler<PageDeleted>,
        IEventHandler<PageActivated>,
        IEventHandler<PageDetailsUpdated>,
        IEventHandler<PageHidden>,
        IEventHandler<PageModuleAdded>,
        IEventHandler<PageModuleDetailsUpdated>,
        IEventHandler<PageModulesReordered>,
        IEventHandler<PageModuleRemoved>
    {
        private readonly ICacheManager _cacheManager;
        private readonly ILanguageFacade _languageFacade;

        public PageEventsHandler(ICacheManager cacheManager, 
            ILanguageFacade languageFacade)
        {
            _cacheManager = cacheManager;
            _languageFacade = languageFacade;
        }

        public void Handle(PageCreated @event)
        {
        }

        public void Handle(PageDetailsUpdated @event)
        {
            ClearPageCache(@event.SiteId, @event.AggregateRootId);
        }

        public void Handle(PageDeleted @event)
        {
            ClearPageCache(@event.SiteId, @event.AggregateRootId);
        }

        public void Handle(PageActivated @event)
        {
            ClearPageCache(@event.SiteId, @event.AggregateRootId);
            ClearMenuCache(@event.SiteId);
        }

        public void Handle(PageHidden @event)
        {
            ClearPageCache(@event.SiteId, @event.AggregateRootId);
            ClearMenuCache(@event.SiteId);
        }

        public void Handle(PageModuleAdded @event)
        {
            ClearPageCache(@event.SiteId, @event.AggregateRootId);
        }

        public void Handle(PageModuleDetailsUpdated @event)
        {
            ClearPageCache(@event.SiteId, @event.AggregateRootId);
        }

        public void Handle(PageModulesReordered @event)
        {
            ClearPageCache(@event.SiteId, @event.AggregateRootId);
        }

        public void Handle(PageModuleRemoved @event)
        {
            ClearPageCache(@event.SiteId, @event.AggregateRootId);
        }

        private void ClearPageCache(Guid siteId, Guid pageId)
        {
            foreach (var language in _languageFacade.GetAllActiveAsync(siteId).Result)
                _cacheManager.Remove(string.Format(CacheKeys.PageInfoCacheKey, siteId, pageId, language.Id));

            _cacheManager.Remove(string.Format(CacheKeys.PageInfoCacheKey, siteId, pageId, Guid.Empty));
        }

        private void ClearMenuCache(Guid siteId)
        {
            foreach (var language in _languageFacade.GetAllActiveAsync(siteId).Result)
                _cacheManager.Remove(string.Format(CacheKeys.MenuCacheKey, siteId, "Main", language.Id));

            _cacheManager.Remove(string.Format(CacheKeys.MenuCacheKey, siteId, "Main", Guid.Empty));
        }
    }
}
