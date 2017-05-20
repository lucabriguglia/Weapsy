using System;
using System.Collections.Generic;
using Weapsy.Domain.Pages.Events;
using Weapsy.Framework.Caching;
using Weapsy.Framework.Events;
using Weapsy.Framework.Queries;
using Weapsy.Reporting.Languages;
using Weapsy.Reporting.Languages.Queries;

namespace Weapsy.Data.Reporting.Pages
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
        private readonly IQueryDispatcher _queryDispatcher;

        public PageEventsHandler(ICacheManager cacheManager, 
            IQueryDispatcher queryDispatcher)
        {
            _cacheManager = cacheManager;
            _queryDispatcher = queryDispatcher;
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
            var languages = _queryDispatcher.DispatchAsync<GetAllActive, IEnumerable<LanguageInfo>>(new GetAllActive { SiteId = siteId }).Result;
            foreach (var language in languages)
                _cacheManager.Remove(string.Format(CacheKeys.PageInfoCacheKey, siteId, pageId, language.Id));

            _cacheManager.Remove(string.Format(CacheKeys.PageInfoCacheKey, siteId, pageId, Guid.Empty));
        }

        private void ClearMenuCache(Guid siteId)
        {
            var languages = _queryDispatcher.DispatchAsync<GetAllActive, IEnumerable<LanguageInfo>>(new GetAllActive { SiteId = siteId }).Result;
            foreach (var language in languages)
                _cacheManager.Remove(string.Format(CacheKeys.MenuCacheKey, siteId, "Main", language.Id));

            _cacheManager.Remove(string.Format(CacheKeys.MenuCacheKey, siteId, "Main", Guid.Empty));
        }
    }
}
