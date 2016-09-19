namespace Weapsy.Domain.Themes
{
    public class ThemeSortOrderGenerator : IThemeSortOrderGenerator
    {
        private readonly IThemeRepository _themeRepository;

        public ThemeSortOrderGenerator(IThemeRepository themeRepository)
        {
            _themeRepository = themeRepository;
        }

        public int GenerateNextSortOrder()
        {
            var themesCount = _themeRepository.GetThemesCount();
            return themesCount + 1;
        }
    }
}
