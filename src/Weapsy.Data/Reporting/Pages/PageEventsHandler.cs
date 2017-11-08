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
        IEventHandler<PageCreatedEvent>,
        IEventHandler<PageDeletedEvent>,
        IEventHandler<PageActivatedEvent>,
        IEventHandler<PageDetailsUpdatedEvent>,
        IEventHandler<PageHiddenEvent>,
        IEventHandler<PageModuleAddedEvent>,
        IEventHandler<PageModuleDetailsUpdatedEvent>,
        IEventHandler<PageModulesReorderedEvent>,
        IEventHandler<PageModuleRemovedEvent>
    {
        private readonly ICacheManager _cacheManager;
        private readonly IQueryDispatcher _queryDispatcher;

        public PageEventsHandler(ICacheManager cacheManager, 
            IQueryDispatcher queryDispatcher)
        {
            _cacheManager = cacheManager;
            _queryDispatcher = queryDispatcher;
        }

        public void Handle(PageCreatedEvent @event)
        {
        }

        public void Handle(PageDetailsUpdatedEvent @event)
        {
            ClearPageCache(@event.SiteId, @event.AggregateRootId);
        }

        public void Handle(PageDeletedEvent @event)
        {
            ClearPageCache(@event.SiteId, @event.AggregateRootId);
        }

        public void Handle(PageActivatedEvent @event)
        {
            ClearPageCache(@event.SiteId, @event.AggregateRootId);
            ClearMenuCache(@event.SiteId);
        }

        public void Handle(PageHiddenEvent @event)
        {
            ClearPageCache(@event.SiteId, @event.AggregateRootId);
            ClearMenuCache(@event.SiteId);
        }

        public void Handle(PageModuleAddedEvent @event)
        {
            ClearPageCache(@event.SiteId, @event.AggregateRootId);
        }

        public void Handle(PageModuleDetailsUpdatedEvent @event)
        {
            ClearPageCache(@event.SiteId, @event.AggregateRootId);
        }

        public void Handle(PageModulesReorderedEvent @event)
        {
            ClearPageCache(@event.SiteId, @event.AggregateRootId);
        }

        public void Handle(PageModuleRemovedEvent @event)
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
