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
            foreach (var language in _languageFacade.GetAllActive(siteId))
                _cacheManager.Remove(string.Format(CacheKeys.MenuCacheKey, siteId, name, language.Id));

            _cacheManager.Remove(string.Format(CacheKeys.MenuCacheKey, siteId, name, Guid.Empty));
        }
    }
}
