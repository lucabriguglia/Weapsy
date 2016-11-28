using System;
using System.Linq;
using AutoMapper;
using Weapsy.Domain.Languages;
using Weapsy.Domain.Pages;
using Weapsy.Domain.Sites;
using Weapsy.Infrastructure.Caching;
using Weapsy.Reporting.Pages;
using Weapsy.Reporting.Sites;

namespace Weapsy.Reporting.Data.Sites
{
    public class SiteFacade : ISiteFacade
    {
        private readonly ISiteRepository _siteRepository;
        private readonly ILanguageRepository _languageRepository;
        private readonly IPageFacade _pageFacade;
        private readonly ICacheManager _cacheManager;
        private readonly IMapper _mapper;

        public SiteFacade(ISiteRepository siteRepository,
            ILanguageRepository languageRepository,
            IPageFacade pageFacade,
            ICacheManager cacheManager,
            IMapper mapper)
        {
            _siteRepository = siteRepository;
            _languageRepository = languageRepository;
            _pageFacade = pageFacade;
            _cacheManager = cacheManager;
            _mapper = mapper;
        }

        public SiteInfo GetSiteInfo(string name, Guid languageId = new Guid())
        {
            return _cacheManager.Get(string.Format(CacheKeys.SiteInfoCacheKey, name, languageId), () =>
            {
                var site = _siteRepository.GetByName(name);

                if (site == null)
                    return null;

                var siteInfo = _mapper.Map<SiteInfo>(site);

                var title = site.Title;
                var metaDescription = site.MetaDescription;
                var metaKeywords = site.MetaKeywords;

                if (languageId != Guid.Empty)
                {
                    var siteLocalisation = site.SiteLocalisations.FirstOrDefault(x => x.LanguageId == languageId);

                    if (siteLocalisation != null)
                    {
                        title = !string.IsNullOrWhiteSpace(siteLocalisation.Title) ? siteLocalisation.Title : title;
                        metaDescription = !string.IsNullOrWhiteSpace(siteLocalisation.MetaDescription) ? siteLocalisation.MetaDescription : metaDescription;
                        metaKeywords = !string.IsNullOrWhiteSpace(siteLocalisation.MetaKeywords) ? siteLocalisation.MetaKeywords : metaKeywords;
                    }
                }

                siteInfo.Title = title;
                siteInfo.MetaDescription = metaDescription;
                siteInfo.MetaKeywords = metaKeywords;

                return siteInfo;
            });
        }

        public SiteAdminModel GetAdminModel(Guid id)
        {
            var site = _siteRepository.GetById(id);

            if (site == null)
                return null;

            var model = _mapper.Map<SiteAdminModel>(site);

            model.SiteLocalisations.Clear();

            foreach (var language in _languageRepository.GetAll(id))
            {
                var title = string.Empty;
                var metaDescription = string.Empty;
                var metaKeywords = string.Empty;

                var existingLocalisation = site
                    .SiteLocalisations
                    .FirstOrDefault(x => x.LanguageId == language.Id);

                if (existingLocalisation != null)
                {
                    title = existingLocalisation.Title;
                    metaDescription = existingLocalisation.MetaDescription;
                    metaKeywords = existingLocalisation.MetaKeywords;
                }

                model.SiteLocalisations.Add(new SiteLocalisationAdminModel
                {
                    SiteId = site.Id,
                    LanguageId = language.Id,
                    LanguageName = language.Name,
                    LanguageStatus = language.Status,
                    Title = title,
                    MetaDescription = metaDescription,
                    MetaKeywords = metaKeywords
                });
            }

            foreach (var page in _pageFacade.GetAllForAdmin(id).Where(x => x.Status == PageStatus.Active))
            {
                model.Pages.Add(new PageListAdminModel
                {
                    Id = page.Id,
                    Name = page.Name
                });
            }

            return model;
        }
    }
}
