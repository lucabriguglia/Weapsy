using System;
using Weapsy.Apps.Text.Domain.Events;
using Weapsy.Infrastructure.Caching;
using Weapsy.Infrastructure.Dispatcher;
using Weapsy.Infrastructure.Domain;
using Weapsy.Reporting.Languages;

namespace Weapsy.Apps.Text.Data
{
    public class TextModuleEventsHandler : 
        IEventHandler<TextModuleCreated>,
        IEventHandler<VersionAdded>       
    {
        private readonly ICacheManager _cacheManager;
        private readonly ILanguageFacade _languageFacade;

        public TextModuleEventsHandler(ICacheManager cacheManager, 
            ILanguageFacade languageFacade)
        {
            _cacheManager = cacheManager;
            _languageFacade = languageFacade;
        }

        public void Handle(TextModuleCreated @event)
        {
            ClearCache(@event.SiteId, @event.ModuleId);
        }

        public void Handle(VersionAdded @event)
        {
            ClearCache(@event.SiteId, @event.ModuleId);
        }

        private void ClearCache(Guid siteId, Guid moduleId)
        {
            foreach (var language in _languageFacade.GetAllActiveAsync(siteId).Result)
                _cacheManager.Remove(string.Format(CacheKeys.TextModuleCacheKey, moduleId, language.Id));

            _cacheManager.Remove(string.Format(CacheKeys.TextModuleCacheKey, moduleId, Guid.Empty));
        }
    }
}
