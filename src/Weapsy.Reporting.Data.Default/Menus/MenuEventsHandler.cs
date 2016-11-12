using System;
using System.Threading.Tasks;
using Weapsy.Infrastructure.Caching;
using Weapsy.Infrastructure.Domain;
using Weapsy.Domain.Menus.Events;
using Weapsy.Reporting.Languages;

namespace Weapsy.Reporting.Data.Default.Menus
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
        private readonly ILanguageFacade _languageFacade;

        public MenuEventsHandler(ICacheManager cacheManager, 
            ILanguageFacade languageFacade)
        {
            _cacheManager = cacheManager;
            _languageFacade = languageFacade;
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

        public async Task Handle(MenuItemRemoved @event)
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
            return Task.Run(() =>
            {
                foreach (var language in _languageFacade.GetAllActive(siteId))
                    _cacheManager.Remove(string.Format(CacheKeys.MenuCacheKey, siteId, name, language.Id));

                _cacheManager.Remove(string.Format(CacheKeys.MenuCacheKey, siteId, name, Guid.Empty));
            });
        }
    }
}
