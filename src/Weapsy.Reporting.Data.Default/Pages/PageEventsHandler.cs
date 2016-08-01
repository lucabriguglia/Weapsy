using System;
using System.Threading.Tasks;
using Weapsy.Core.Caching;
using Weapsy.Core.Domain;
using Weapsy.Domain.Model.Pages.Events;

namespace Weapsy.Reporting.Data.Default.Pages
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

        public PageEventsHandler(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }

        public async Task Handle(PageCreated @event)
        {
        }

        public async Task Handle(PageDetailsUpdated @event)
        {
            await ClearPageCache(@event.SiteId, @event.AggregateRootId);
        }

        public async Task Handle(PageDeleted @event)
        {
            await ClearPageCache(@event.SiteId, @event.AggregateRootId);
        }

        public async Task Handle(PageActivated @event)
        {
            await ClearPageCache(@event.SiteId, @event.AggregateRootId);
            await ClearMenuCache(@event.SiteId);
        }

        public async Task Handle(PageHidden @event)
        {
            await ClearPageCache(@event.SiteId, @event.AggregateRootId);
            await ClearMenuCache(@event.SiteId);
        }

        public async Task Handle(PageModuleAdded @event)
        {
            await ClearPageCache(@event.SiteId, @event.AggregateRootId);
        }

        public async Task Handle(PageModuleDetailsUpdated @event)
        {
            await ClearPageCache(@event.SiteId, @event.AggregateRootId);
        }

        public async Task Handle(PageModulesReordered @event)
        {
            await ClearPageCache(@event.SiteId, @event.AggregateRootId);
        }

        public async Task Handle(PageModuleRemoved @event)
        {
            await ClearPageCache(@event.SiteId, @event.AggregateRootId);
        }

        private Task ClearPageCache(Guid siteId, Guid pageId)
        {
            return Task.Run(() => _cacheManager.Remove(string.Format(CacheKeys.PageCacheKey, siteId, pageId)));
        }

        private Task ClearMenuCache(Guid siteId)
        {
            return Task.Run(() => _cacheManager.Remove(string.Format(CacheKeys.MenuCacheKey, siteId, "Main")));
        }
    }
}
