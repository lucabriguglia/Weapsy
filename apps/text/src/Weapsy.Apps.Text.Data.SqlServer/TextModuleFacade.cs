using System;
using System.Linq;
using Weapsy.Apps.Text.Data.SqlServer;
using Weapsy.Apps.Text.Domain;
using Weapsy.Core.Caching;
using Weapsy.Domain.Languages;
using Weapsy.Domain.Modules;

namespace Weapsy.Apps.Text.Reporting.Data.Default
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

        public string GetContent(Guid moduleId)
        {
            return _cacheManager.Get(string.Format(CacheKeys.TextModuleCacheKey, moduleId), () =>
            {
                var textModule = _textRepository.GetByModuleId(moduleId);

                if (textModule == null)
                    return null;

                var publishedVersion = textModule.TextVersions.FirstOrDefault(x => x.Status == TextVersionStatus.Published);

                return publishedVersion != null ? publishedVersion.Content : string.Empty;
            });
        }

        public TextModuleAdminModel GetAdminModel(Guid siteId, Guid moduleId, Guid versionId = new Guid())
        {
            var module = _moduleRepository.GetById(siteId, moduleId);

            if (module == null || module.Status == ModuleStatus.Deleted)
                return new TextModuleAdminModel();

            var textModule = _textRepository.GetByModuleId(moduleId);

            if (textModule == null || textModule.Status == TextModuleStatus.Deleted)
                return new TextModuleAdminModel();

            var version = versionId != Guid.Empty 
                ? textModule.TextVersions.FirstOrDefault(x => x.Id == versionId) 
                : textModule.TextVersions.FirstOrDefault(x => x.Status == TextVersionStatus.Published);

            if (version == null || version.Status == TextVersionStatus.Deleted)
                return new TextModuleAdminModel();

            var result = new TextModuleAdminModel
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

                result.VersionLocalisations.Add(new TextModuleAdminModel.VersionLocalisation
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
