using System;
using System.Threading.Tasks;
using Weapsy.Apps.Text.Domain.Events;
using Weapsy.Infrastructure.Caching;
using Weapsy.Infrastructure.Domain;
using Weapsy.Reporting.Languages;

namespace Weapsy.Apps.Text.Data.SqlServer
{
    public class TextModuleEventsHandler : 
        IEventHandlerAsync<TextModuleCreated>,
        IEventHandlerAsync<VersionAdded>       
    {
        private readonly ICacheManager _cacheManager;
        private readonly ILanguageFacade _languageFacade;

        public TextModuleEventsHandler(ICacheManager cacheManager, 
            ILanguageFacade languageFacade)
        {
            _cacheManager = cacheManager;
            _languageFacade = languageFacade;
        }

        public Task HandleAsync(TextModuleCreated @event)
        {
            return ClearCache(@event.SiteId, @event.ModuleId);
        }

        public Task HandleAsync(VersionAdded @event)
        {
            return ClearCache(@event.SiteId, @event.ModuleId);
        }

        private Task ClearCache(Guid siteId, Guid moduleId)
        {
            return Task.Run(() =>
            {
                foreach (var language in _languageFacade.GetAllActive(siteId))
                    _cacheManager.Remove(string.Format(CacheKeys.TextModuleCacheKey, moduleId, language.Id));

                _cacheManager.Remove(string.Format(CacheKeys.TextModuleCacheKey, moduleId, Guid.Empty));
            });
        }
    }
}
