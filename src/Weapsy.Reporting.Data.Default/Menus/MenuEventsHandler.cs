using System;
using System.Threading.Tasks;
using Weapsy.Core.Caching;
using Weapsy.Core.Domain;
using Weapsy.Domain.Model.Menus.Events;

namespace Weapsy.Reporting.Data.Default.Menus
{
    public class MenuEventsHandler : 
        IEventHandler<MenuCreated>,
        IEventHandler<MenuItemAdded>,
        IEventHandler<MenuItemUpdated>,
        IEventHandler<MenuItemsReordered>,
        IEventHandler<MenuDeleted>        
    {
        private readonly ICacheManager _cacheManager;

        public MenuEventsHandler(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }

        public async Task Handle(MenuCreated @event)
        {
            await ClearCache(@event.SiteId, @event.Name);
        }

        public async Task Handle(MenuItemAdded @event)
        {
            await ClearCache(@event.SiteId, @event.Name);
        }

        public async Task Handle(MenuItemUpdated @event)
        {
            await ClearCache(@event.SiteId, @event.Name);
        }

        public async Task Handle(MenuItemsReordered @event)
        {
            await ClearCache(@event.SiteId, @event.Name);
        }

        public async Task Handle(MenuDeleted @event)
        {
            await ClearCache(@event.SiteId, @event.Name);
        }

        private Task ClearCache(Guid siteId, string name)
        {
            return Task.Run(() => _cacheManager.Remove(string.Format(CacheKeys.MenuCacheKey, siteId, name)));
        }
    }
}
