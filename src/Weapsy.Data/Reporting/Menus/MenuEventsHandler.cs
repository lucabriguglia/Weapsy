using System;
using System.Collections.Generic;
using Weapsy.Cqrs;
using Weapsy.Cqrs.Events;
using Weapsy.Data.Caching;
using Weapsy.Domain.Menus.Events;
using Weapsy.Reporting.Languages;
using Weapsy.Reporting.Languages.Queries;

namespace Weapsy.Data.Reporting.Menus
{
    public class MenuEventsHandler :
        IEventHandler<MenuCreated>,
        IEventHandler<MenuItemAdded>,
        IEventHandler<MenuItemUpdated>,
        IEventHandler<MenuItemRemoved>,
        IEventHandler<MenuItemsReordered>,
        IEventHandler<MenuDeleted>        
    {
        private readonly ICacheManager _cacheManager;
        private readonly IDispatcher _queryDispatcher;

        public MenuEventsHandler(ICacheManager cacheManager, 
            IDispatcher queryDispatcher)
        {
            _cacheManager = cacheManager;
            _queryDispatcher = queryDispatcher;
        }

        public void Handle(MenuCreated @event)
        {
            ClearCache(@event.SiteId, @event.Name);
        }

        public void Handle(MenuItemAdded @event)
        {
            ClearCache(@event.SiteId, @event.Name);
        }

        public void Handle(MenuItemUpdated @event)
        {
            ClearCache(@event.SiteId, @event.MenuName);
        }

        public void Handle(MenuItemRemoved @event)
        {
            ClearCache(@event.SiteId, @event.Name);
        }

        public void Handle(MenuItemsReordered @event)
        {
            ClearCache(@event.SiteId, @event.Name);
        }

        public void Handle(MenuDeleted @event)
        {
            ClearCache(@event.SiteId, @event.Name);
        }

        private void ClearCache(Guid siteId, string name)
        {
            var languages = _queryDispatcher.GetResultAsync<GetAllActive, IEnumerable<LanguageInfo>>(new GetAllActive { SiteId = siteId }).Result;
            foreach (var language in languages)
                _cacheManager.Remove(string.Format(CacheKeys.MenuCacheKey, siteId, name, language.Id));

            _cacheManager.Remove(string.Format(CacheKeys.MenuCacheKey, siteId, name, Guid.Empty));
        }
    }
}
