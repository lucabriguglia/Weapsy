using System;
using System.Collections.Generic;
using AutoMapper;
using Weapsy.Data;
using Weapsy.Domain.Themes;
using Weapsy.Infrastructure.Caching;
using Weapsy.Reporting.Themes;
using System.Linq;

namespace Weapsy.Reporting.Data.Themes
{
    public class ThemeFacade : IThemeFacade
    {
        private readonly IDbContextFactory _dbContextFactory;
        private readonly ICacheManager _cacheManager;
        private readonly IMapper _mapper;

        public ThemeFacade(IDbContextFactory dbContextFactory, 
            ICacheManager cacheManager,
            IMapper mapper)
        {
            _dbContextFactory = dbContextFactory;
            _cacheManager = cacheManager;
            _mapper = mapper;
        }

        public IEnumerable<ThemeAdminModel> GetAllForAdmin()
        {
            using (var context = _dbContextFactory.Create())
            {
                var dbEntities = context.Themes
                    .Where(x => x.Status != ThemeStatus.Deleted)
                    .ToList();

                return _mapper.Map<IEnumerable<ThemeAdminModel>>(dbEntities);
            }
        }

        public ThemeAdminModel GetForAdmin(Guid id)
        {
            using (var context = _dbContextFactory.Create())
            {
                var dbEntity = context.Themes.FirstOrDefault(x => x.Id == id && x.Status != ThemeStatus.Deleted);
                return dbEntity != null ? _mapper.Map<ThemeAdminModel>(dbEntity) : null;
            }
        }
    }
}