using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Weapsy.Data.Identity;
using Weapsy.Domain.Languages;
using Weapsy.Domain.Menus;
using Weapsy.Domain.Pages;
using Weapsy.Infrastructure.Caching;
using Weapsy.Infrastructure.Queries;
using Weapsy.Reporting.Menus;
using Weapsy.Reporting.Menus.Queries;
using Weapsy.Reporting.Pages;
using Weapsy.Reporting.Pages.Queries;
using Language = Weapsy.Data.Entities.Language;
using Menu = Weapsy.Data.Entities.Menu;
using MenuItem = Weapsy.Data.Entities.MenuItem;

namespace Weapsy.Data.Reporting.Pages
{
    public class GetPageInfoHandler : IQueryHandlerAsync<GetPageInfo, PageInfo>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;
        private readonly ICacheManager _cacheManager;
        private readonly IRoleService _roleService;
        private readonly IPageInfoFactory _pageViewFactory;

        public GetPageInfoHandler(IDbContextFactory contextFactory, 
            IMapper mapper, 
            ICacheManager cacheManager, 
            IRoleService roleService, 
            IPageInfoFactory pageViewFactory)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
            _cacheManager = cacheManager;
            _roleService = roleService;
            _pageViewFactory = pageViewFactory;
        }

        public async Task<PageInfo> RetrieveAsync(GetPageInfo query)
        {
            return _cacheManager.Get(string.Format(CacheKeys.PageInfoCacheKey, query.SiteId, query.PageId, query.LanguageId), () =>
            {
                return _pageViewFactory.CreatePageInfo(query.SiteId, query.PageId, query.LanguageId);
            });
        }
    }
}
