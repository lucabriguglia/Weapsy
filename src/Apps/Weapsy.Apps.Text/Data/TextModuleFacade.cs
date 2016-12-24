using System;
using System.Linq;
using Weapsy.Apps.Text.Domain;
using Weapsy.Apps.Text.Domain.Commands;
using Weapsy.Apps.Text.Reporting;
using Weapsy.Domain.Languages;
using Weapsy.Domain.Modules;
using Weapsy.Infrastructure.Caching;

namespace Weapsy.Apps.Text.Data
{
    public class TextModuleFacade : ITextModuleFacade
    {
        private readonly ITextModuleRepository _textRepository;
        private readonly ILanguageRepository _languageRepository;
        private readonly IModuleRepository _moduleRepository;
        private readonly ICacheManager _cacheManager;

        public TextModuleFacade(ITextModuleRepository textRepository, 
            ILanguageRepository languageRepository,
            IModuleRepository moduleRepository,
            ICacheManager cacheManager)
        {
            _textRepository = textRepository;
            _languageRepository = languageRepository;
            _moduleRepository = moduleRepository;
            _cacheManager = cacheManager;
        }

        public string GetContent(Guid moduleId, Guid languageId = new Guid())
        {
            return _cacheManager.Get(string.Format(CacheKeys.TextModuleCacheKey, moduleId, languageId), () =>
            {
                var textModule = _textRepository.GetByModuleId(moduleId);

                if (textModule == null)
                    return null;

                var publishedVersion = textModule.TextVersions.FirstOrDefault(x => x.Status == TextVersionStatus.Published);

                var content = publishedVersion != null ? publishedVersion.Content : string.Empty;

                if (languageId != Guid.Empty)
                {
                    var localisedVersion =
                        publishedVersion.TextLocalisations.FirstOrDefault(x => x.LanguageId == languageId);

                    if (localisedVersion != null && !string.IsNullOrEmpty(localisedVersion.Content))
                        content = localisedVersion.Content;
                }

                return content;
            });
        }

        public AddVersion GetAdminModel(Guid siteId, Guid moduleId, Guid versionId = new Guid())
        {
            var module = _moduleRepository.GetById(siteId, moduleId);

            if (module == null || module.Status == ModuleStatus.Deleted)
                return new AddVersion();

            var textModule = _textRepository.GetByModuleId(moduleId);

            if (textModule == null || textModule.Status == TextModuleStatus.Deleted)
                return new AddVersion();

            var version = versionId != Guid.Empty 
                ? textModule.TextVersions.FirstOrDefault(x => x.Id == versionId) 
                : textModule.TextVersions.FirstOrDefault(x => x.Status == TextVersionStatus.Published);

            if (version == null || version.Status == TextVersionStatus.Deleted)
                return new AddVersion();

            var result = new AddVersion
            {
                Id = textModule.Id,
                ModuleId = textModule.ModuleId,
                Content = version.Content,
                Description = version.Description
            };

            var languages = _languageRepository.GetAll(siteId).Where(x => x.Status != LanguageStatus.Deleted);

            foreach (var language in languages)
            {
                var content = string.Empty;

                var existingLocalisation = version
                    .TextLocalisations
                    .FirstOrDefault(x => x.LanguageId == language.Id);

                if (existingLocalisation != null)
                {
                    content = existingLocalisation.Content;
                }

                result.VersionLocalisations.Add(new AddVersion.VersionLocalisation
                {
                    LanguageId = language.Id,
                    LanguageName = language.Name,
                    Content = content
                });
            }

            return result;
        }
    }
}
