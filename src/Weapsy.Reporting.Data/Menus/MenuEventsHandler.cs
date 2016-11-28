using System;
using System.Threading.Tasks;
using Weapsy.Domain.Menus.Events;
using Weapsy.Infrastructure.Caching;
using Weapsy.Infrastructure.Domain;
using Weapsy.Reporting.Languages;

namespace Weapsy.Reporting.Data.Menus
{
    public class MenuEventsHandler : 
        IEventHandlerAsync<MenuCreated>,
        IEventHandlerAsync<MenuItemAdded>,
        IEventHandlerAsync<MenuItemUpdated>,
        IEventHandlerAsync<MenuItemRemoved>,
        IEventHandlerAsync<MenuItemsReordered>,        
        IEventHandlerAsync<MenuDeleted>        
    {
        private readonly ICacheManager _cacheManager;
        private readonly ILanguageFacade _languageFacade;

        public MenuEventsHandler(ICacheManager cacheManager, 
            ILanguageFacade languageFacade)
        {
            _cacheManager = cacheManager;
            _languageFacade = languageFacade;
        }

        public async Task HandleAsync(MenuCreated @event)
        {
            await ClearCache(@event.SiteId, @event.Name);
        }

        public async Task HandleAsync(MenuItemAdded @event)
        {
            await ClearCache(@event.SiteId, @event.Name);
        }

        public async Task HandleAsync(MenuItemUpdated @event)
        {
            await ClearCache(@event.SiteId, @event.Name);
        }

        public async Task HandleAsync(MenuItemRemoved @event)
        {
            await ClearCache(@event.SiteId, @event.Name);
        }

        public async Task HandleAsync(MenuItemsReordered @event)
        {
            await ClearCache(@event.SiteId, @event.Name);
        }

        public async Task HandleAsync(MenuDeleted @event)
        {
            await ClearCache(@event.SiteId, @event.Name);
        }

        private Task ClearCache(Guid siteId, string name)
        {
            return Task.Run(() =>
            {
                foreach (var language in _languageFacade.GetAllActive(siteId).Result)
                    _cacheManager.Remove(string.Format(CacheKeys.MenuCacheKey, siteId, name, language.Id));

                _cacheManager.Remove(string.Format(CacheKeys.MenuCacheKey, siteId, name, Guid.Empty));
            });
        }
    }
}
