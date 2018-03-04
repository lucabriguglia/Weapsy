using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Weapsy.Cqrs;
using Weapsy.Domain.Languages;
using Weapsy.Domain.Pages;
using Weapsy.Domain.Sites;
using Weapsy.Reporting.Sites;
using Weapsy.Reporting.Sites.Queries;
using Weapsy.Domain.Themes;
using Weapsy.Cqrs.Queries;
using Weapsy.Data.Caching;

namespace Weapsy.Data.Reporting.Sites
{
    public class GetAdminModelHandler : IQueryHandlerAsync<GetAdminModel, SiteAdminModel>
    {
        private readonly IContextFactory _contextFactory;
        private readonly IMapper _mapper;
        private readonly ICacheManager _cacheManager;
        private readonly IDispatcher _queryDispatcher;

        public GetAdminModelHandler(IContextFactory contextFactory, 
            IMapper mapper, 
            ICacheManager cacheManager, 
            IDispatcher queryDispatcher)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
            _cacheManager = cacheManager;
            _queryDispatcher = queryDispatcher;
        }

        public async Task<SiteAdminModel> RetrieveAsync(GetAdminModel query)
        {
            using (var context = _contextFactory.Create())
            {
                var site = await context.Sites
                    .Include(x => x.SiteLocalisations)
                    .FirstOrDefaultAsync(x => x.Id == query.Id && x.Status == SiteStatus.Active);

                if (site == null)
                    return null;

                var model = _mapper.Map<SiteAdminModel>(site);

                model.SiteLocalisations.Clear();

                var languages = await context.Languages
                    .Where(x => x.SiteId == site.Id && x.Status != LanguageStatus.Deleted)
                    .OrderBy(x => x.SortOrder)
                    .ToListAsync();

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

                var pages = await context.Pages
                    .Where(x => x.SiteId == site.Id && x.Status == PageStatus.Active)
                    .Select(page => new PageListAdminModel
                    {
                        Id = page.Id,
                        Name = page.Name
                    }).ToListAsync();

                model.Pages.AddRange(pages);

                var themes = await context.Themes
                    .Where(x => x.Status == ThemeStatus.Active)
                    .Select(theme => new ThemeListAdminModel
                    {
                        Id = theme.Id,
                        Name = theme.Name
                    }).ToListAsync();

                model.Themes.AddRange(themes);

                return model;
            }
        }
    }
}
