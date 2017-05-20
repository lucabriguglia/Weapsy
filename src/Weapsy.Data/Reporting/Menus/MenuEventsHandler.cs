using System;
using System.Collections.Generic;
using Weapsy.Domain.Menus.Events;
using Weapsy.Framework.Caching;
using Weapsy.Framework.Events;
using Weapsy.Framework.Queries;
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
        private readonly IQueryDispatcher _queryDispatcher;

        public MenuEventsHandler(ICacheManager cacheManager, 
            IQueryDispatcher queryDispatcher)
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
            ClearCache(@event.SiteId, @event.Name);
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
            var languages = _queryDispatcher.DispatchAsync<GetAllActive, IEnumerable<LanguageInfo>>(new GetAllActive { SiteId = siteId }).Result;
            foreach (var language in languages)
                _cacheManager.Remove(string.Format(CacheKeys.MenuCacheKey, siteId, name, language.Id));

            _cacheManager.Remove(string.Format(CacheKeys.MenuCacheKey, siteId, name, Guid.Empty));
        }
    }
}
