using System;
using System.Collections.Generic;
using Weapsy.Apps.Text.Domain.Events;
using Weapsy.Cqrs;
using Weapsy.Cqrs.Events;
using Weapsy.Data.Caching;
using Weapsy.Reporting.Languages;
using Weapsy.Reporting.Languages.Queries;

namespace Weapsy.Apps.Text.Data.Reporting
{
    public class TextModuleEventsHandler : 
        IEventHandler<TextModuleCreated>,
        IEventHandler<VersionAdded>       
    {
        private readonly ICacheManager _cacheManager;
        private readonly IDispatcher _dispatcher;

        public TextModuleEventsHandler(ICacheManager cacheManager, 
            IDispatcher queryDispatcher)
        {
            _cacheManager = cacheManager;
            _dispatcher = queryDispatcher;
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
            var languages = _dispatcher.GetResultAsync<GetAllActive, IEnumerable<LanguageInfo>>(new GetAllActive { SiteId = siteId }).Result;
            foreach (var language in languages)
                _cacheManager.Remove(string.Format(CacheKeys.TextModuleCacheKey, moduleId, language.Id));

            _cacheManager.Remove(string.Format(CacheKeys.TextModuleCacheKey, moduleId, Guid.Empty));
        }
    }
}
