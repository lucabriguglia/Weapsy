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
        IEventHandler<MenuCreatedEvent>,
        IEventHandler<MenuItemAddedEvent>,
        IEventHandler<MenuItemUpdatedEvent>,
        IEventHandler<MenuItemRemovedEvent>,
        IEventHandler<MenuItemsReorderedEvent>,
        IEventHandler<MenuDeletedEvent>        
    {
        private readonly ICacheManager _cacheManager;
        private readonly IQueryDispatcher _queryDispatcher;

        public MenuEventsHandler(ICacheManager cacheManager, 
            IQueryDispatcher queryDispatcher)
        {
            _cacheManager = cacheManager;
            _queryDispatcher = queryDispatcher;
        }

        public void Handle(MenuCreatedEvent @event)
        {
            ClearCache(@event.SiteId, @event.Name);
        }

        public void Handle(MenuItemAddedEvent @event)
        {
            ClearCache(@event.SiteId, @event.Name);
        }

        public void Handle(MenuItemUpdatedEvent @event)
        {
            ClearCache(@event.SiteId, @event.Name);
        }

        public void Handle(MenuItemRemovedEvent @event)
        {
            ClearCache(@event.SiteId, @event.Name);
        }

        public void Handle(MenuItemsReorderedEvent @event)
        {
            ClearCache(@event.SiteId, @event.Name);
        }

        public void Handle(MenuDeletedEvent @event)
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
