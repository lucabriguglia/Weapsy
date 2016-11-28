using System;
using System.Collections.Generic;
using AutoMapper;
using Weapsy.Domain.Themes;
using Weapsy.Infrastructure.Caching;
using Weapsy.Reporting.Themes;

namespace Weapsy.Reporting.Data.Themes
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
            var themes = _themeRepository.GetAll();
            return _mapper.Map<IEnumerable<ThemeAdminModel>>(themes);
        }

        public ThemeAdminModel GetForAdmin(Guid id)
        {
            var theme = _themeRepository.GetById(id);
            return theme == null ? null : _mapper.Map<ThemeAdminModel>(theme);
        }
    }
}