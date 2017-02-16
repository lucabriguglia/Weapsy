using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Weapsy.Data.Identity;
using Weapsy.Domain.Pages;
using Weapsy.Infrastructure.Caching;
using Weapsy.Infrastructure.Queries;
using Weapsy.Reporting.Pages.Queries;

namespace Weapsy.Data.Reporting.Pages
{
    public class GetPageIdBySlugHandler : IQueryHandlerAsync<GetPageIdBySlug, Guid?>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;
        private readonly ICacheManager _cacheManager;
        private readonly IRoleService _roleService;

        public GetPageIdBySlugHandler(IDbContextFactory contextFactory, 
            IMapper mapper, 
            ICacheManager cacheManager, 
            IRoleService roleService)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
            _cacheManager = cacheManager;
            _roleService = roleService;
        }

        public async Task<Guid?> RetrieveAsync(GetPageIdBySlug query)
        {
            using (var context = _contextFactory.Create())
            {
                if (query.LanguageId == Guid.Empty)
                {
                    var dbEntity = context.Pages
                        .FirstOrDefault(x => x.SiteId == query.SiteId
                        && x.Status == PageStatus.Active
                        && x.Url == query.Slug);

                    return dbEntity?.Id;
                }
                else
                {
                    var dbEntity = context.PageLocalisations
                        .FirstOrDefault(x => x.Page.SiteId == query.SiteId
                        && x.Page.Status == PageStatus.Active
                        && x.Url == query.Slug
                        && x.LanguageId == query.LanguageId);

                    return dbEntity?.PageId;
                }
            }
        }
    }
}
