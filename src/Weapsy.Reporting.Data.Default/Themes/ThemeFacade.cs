using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Weapsy.Infrastructure.Caching;
using Weapsy.Domain.Themes;
using Weapsy.Reporting.Themes;

namespace Weapsy.Reporting.Data.Default.Themes
{
    public class ThemeFacade : IThemeFacade
    {
        private readonly IThemeRepository _themeRepository;
        private readonly ICacheManager _cacheManager;
        private readonly IMapper _mapper;

        public ThemeFacade(IThemeRepository themeRepository, 
            ICacheManager cacheManager,
            IMapper mapper)
        {
            _themeRepository = themeRepository;
            _cacheManager = cacheManager;
            _mapper = mapper;
        }

        public IEnumerable<ThemeAdminModel> GetAllForAdmin()
        {
            var themes = _themeRepository.GetAll().Where(x => x.Status != ThemeStatus.Deleted);
            return _mapper.Map<IEnumerable<ThemeAdminModel>>(themes);
        }

        public ThemeAdminModel GetForAdmin(Guid id)
        {
            var theme = _themeRepository.GetById(id);
            if (theme == null || theme.Status == ThemeStatus.Deleted)
                return null;
            return _mapper.Map<ThemeAdminModel>(theme);
        }
    }
}