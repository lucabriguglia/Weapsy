using System;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Weapsy.Data;
using Weapsy.Domain.Languages;
using Weapsy.Domain.Pages;
using Weapsy.Domain.Sites;
using Weapsy.Infrastructure.Caching;
using Weapsy.Reporting.Sites;

namespace Weapsy.Reporting.Data.Sites
{
    public class SiteFacade : ISiteFacade
    {
        private readonly IDbContextFactory _dbContextFactory;
        private readonly ICacheManager _cacheManager;
        private readonly IMapper _mapper;

        public SiteFacade(IDbContextFactory dbContextFactory,
            ICacheManager cacheManager,
            IMapper mapper)
        {
            _dbContextFactory = dbContextFactory;
            _cacheManager = cacheManager;
            _mapper = mapper;
        }

        public SiteInfo GetSiteInfo(string name, Guid languageId = new Guid())
        {
            return _cacheManager.Get(string.Format(CacheKeys.SiteInfoCacheKey, name, languageId), () =>
            {
                using (var context = _dbContextFactory.Create())
                {
                    var site = context.Sites
                        .Include(x => x.SiteLocalisations)
                        .FirstOrDefault(x => x.Name == name && x.Status == SiteStatus.Active);

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
                }
            });
        }

        public SiteAdminModel GetAdminModel(Guid id)
        {
            using (var context = _dbContextFactory.Create())
            {
                var site = context.Sites
                    .Include(x => x.SiteLocalisations)
                    .FirstOrDefault(x => x.Id == id && x.Status == SiteStatus.Active);

                if (site == null)
                    return null;

                var model = _mapper.Map<SiteAdminModel>(site);

                model.SiteLocalisations.Clear();

                var languages = context.Languages
                    .Where(x => x.SiteId == site.Id && x.Status != LanguageStatus.Deleted)
                    .OrderBy(x => x.SortOrder)
                    .ToList();

                foreach (var language in languages)
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

                var pages = context.Pages
                    .Where(x => x.SiteId == site.Id && x.Status == PageStatus.Active)
                    .Select(page => new PageListAdminModel
                    {
                        Id = page.Id,
                        Name = page.Name
                    }).ToList();

                model.Pages.AddRange(pages);

                return model;
            }
        }
    }
}
