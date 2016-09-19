using System;
using System.Text.RegularExpressions;

namespace Weapsy.Domain.Themes.Rules
{
    public class ThemeRules : IThemeRules
    {
        private readonly IThemeRepository _themeRepository;

        public ThemeRules(IThemeRepository themeRepository)
        {
            _themeRepository = themeRepository;
        }

        public bool DoesThemeExist(Guid id)
        {
            var theme = _themeRepository.GetById(id);
            return theme != null && theme.Status != ThemeStatus.Deleted;
        }

        public bool IsThemeIdUnique(Guid id)
        {
            return _themeRepository.GetById(id) == null;
        }

        public bool IsThemeNameUnique(string name, Guid themeId = new Guid())
        {
            var theme = _themeRepository.GetByName(name);
            return IsThemeUnique(theme, themeId);
        }

        public bool IsThemeFolderValid(string folder)
        {
            if (string.IsNullOrWhiteSpace(folder)) return false;
            var regex = new Regex(@"^[A-Za-z\d_-]+$");
            var match = regex.Match(folder);
            return match.Success;
        }

        public bool IsThemeFolderUnique(string folder, Guid themeId = new Guid())
        {
            var theme = _themeRepository.GetByFolder(folder);
            return IsThemeUnique(theme, themeId);
        }

        private bool IsThemeUnique(Theme theme, Guid themeId)
        {
            return theme == null
                || theme.Status == ThemeStatus.Deleted
                || (themeId != Guid.Empty && theme.Id == themeId);
        }
    }
}
