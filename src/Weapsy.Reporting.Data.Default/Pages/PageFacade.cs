using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Weapsy.Core.Caching;
using Weapsy.Domain.Model.Languages;
using Weapsy.Domain.Model.Modules;
using Weapsy.Domain.Model.ModuleTypes;
using Weapsy.Domain.Model.Pages;
using Weapsy.Reporting.Pages;

namespace Weapsy.Reporting.Data.Pages
{
    public class PageFacade : IPageFacade
    {
        private readonly IPageRepository _pageRepository;
        private readonly ILanguageRepository _languageRepository;
        private readonly IModuleRepository _moduleRepository;
        private readonly IModuleTypeRepository _moduleTypeRepository;
        private readonly ICacheManager _cacheManager;
        private readonly IMapper _mapper;

        public PageFacade(IPageRepository pageRepository, 
            ILanguageRepository languageRepository,
            IModuleRepository moduleRepository,
            IModuleTypeRepository moduleTypeRepository,
            ICacheManager cacheManager, 
            IMapper mapper)
        {
            _pageRepository = pageRepository;
            _languageRepository = languageRepository;
            _moduleRepository = moduleRepository;
            _moduleTypeRepository = moduleTypeRepository;
            _cacheManager = cacheManager;
            _mapper = mapper;
        }

        public PageViewModel GetPageViewModel(Guid siteId, Guid pageId)
        {
            return _cacheManager.Get(string.Format(CacheKeys.PAGE_CACHE_KEY, siteId, pageId), () =>
            {
                var page = _pageRepository.GetById(siteId, pageId);

                if (page == null || page.Status != PageStatus.Active)
                    return null;

                var result = new PageViewModel();

                result.Page = new PageModel
                {
                    Id = page.Id,
                    Name = page.Name,
                    Url = page.Url,
                    Title = page.Title,
                    MetaDescription = page.MetaDescription,
                    MetaKeywords = page.MetaKeywords
                };

                result.Page.Template = new PageTemplateModel
                {
                    ViewName = "Default"
                };

                //var allPageModules = page.PageModules;
                var zones = page.PageModules.Where(x => x.Status == PageModuleStatus.Active).GroupBy(x => x.Zone);

                foreach (var zone in zones)
                {
                    var zoneModel = new ZoneModel
                    {
                        Name = zone.Key
                    };

                    foreach (var pageModule in zone.OrderBy(x => x.SortOrder))
                    {
                        var moduleModel = BuildModuleModel(pageModule);

                        if (moduleModel == null)
                            continue;

                        zoneModel.Modules.Add(moduleModel);

                        //var pageModuleToRemove = allPageModules.FirstOrDefault(x => x.ModuleId == moduleModel.Id);
                        //if (pageModuleToRemove != null)
                        //    allPageModules.Remove(pageModuleToRemove);
                    }

                    result.Zones.Add(zoneModel);
                }

                //if (allPageModules.Count > 0 && result.Zones.Count > 0)
                //{
                //    foreach (var pageModule in allPageModules)
                //    {
                //        var moduleModel = BuildModuleModel(pageModule);

                //        if (moduleModel == null)
                //            continue;

                //        result.Zones.FirstOrDefault().Modules.Add(moduleModel);
                //    }
                //}

                return result;
            });
        }

        private ModuleModel BuildModuleModel(PageModule pageModule)
        {
            var module = _moduleRepository.GetById(pageModule.ModuleId);

            if (module == null)
                return null;

            var moduleType = _moduleTypeRepository.GetById(module.ModuleTypeId);

            if (moduleType == null)
                return null;

            var moduleModel = new ModuleModel
            {
                Id = pageModule.ModuleId,
                Title = pageModule.Title,
                SortOrder = pageModule.SortOrder
            };

            moduleModel.ModuleType = new ModuleTypeModel
            {
                ViewType = moduleType.ViewType,
                ViewName = moduleType.ViewName,
                EditType = moduleType.EditType,
                EditUrl = moduleType.EditUrl
            };

            moduleModel.Template = new ModuleTemplateModel
            {
                ViewName = "Default"
            };

            return moduleModel;
        }

        public PageViewModel GetPageViewModel(Guid siteId, string name)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<PageAdminListModel>> GetAllForAdminAsync(Guid siteId)
        {
            var pages = _pageRepository.GetAll(siteId).Where(x => x.Status != PageStatus.Deleted);
            return _mapper.Map<IEnumerable<PageAdminListModel>>(pages);
        }

        public async Task<PageAdminModel> GetAdminModelAsync(Guid siteId, Guid pageId)
        {
            var page = _pageRepository.GetById(siteId, pageId);

            if (page == null || page.Status == PageStatus.Deleted)
                return null;

            var result = new PageAdminModel
            {
                Id = page.Id,
                Name = page.Name,
                Status = page.Status,
                Url = page.Url,
                Title = page.Title,
                MetaDescription = page.MetaDescription,
                MetaKeywords = page.MetaKeywords
            };

            var languages = _languageRepository.GetAll(siteId).Where(x => x.Status != LanguageStatus.Deleted);

            foreach (var language in languages)
            {
                var url = string.Empty;
                var Title = string.Empty;
                var metaDescription = string.Empty;
                var metaKeywords = string.Empty;

                var existingLocalisation = page
                    .PageLocalisations
                    .FirstOrDefault(x => x.LanguageId == language.Id);

                if (existingLocalisation != null)
                {
                    url = existingLocalisation.Url;
                    Title = existingLocalisation.Title;
                    metaDescription = existingLocalisation.MetaDescription;
                    metaKeywords = existingLocalisation.MetaKeywords;
                }

                result.PageLocalisations.Add(new PageLocalisationAdminModel
                {
                    PageId = page.Id,
                    LanguageId = language.Id,
                    LanguageName = language.Name,
                    Url = url,
                    Title = Title,
                    MetaDescription = metaDescription,
                    MetaKeywords = metaKeywords
                });
            }

            return result;
        }

        public async Task<PageAdminModel> GetDefaultAdminModelAsync(Guid siteId)
        {
            var result = new PageAdminModel();

            var languages = _languageRepository.GetAll(siteId).Where(x => x.Status != LanguageStatus.Deleted);

            foreach (var language in languages)
            {
                result.PageLocalisations.Add(new PageLocalisationAdminModel
                {
                    LanguageId = language.Id,
                    LanguageName = language.Name
                });
            }

            return result;
        }
    }
}