using System;
using Weapsy.Framework.Domain;

namespace Weapsy.Domain.Themes.Rules
{
    public interface IThemeRules : IRules<Theme>
    {
        bool DoesThemeExist(Guid id);
        bool IsThemeIdUnique(Guid id);
        bool IsThemeNameUnique(string name, Guid themeId = new Guid());
        bool IsThemeFolderValid(string folder);
        bool IsThemeFolderUnique(string folder, Guid themeId = new Guid());
    }
}
