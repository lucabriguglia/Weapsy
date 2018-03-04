using Weapsy.Domain.Themes.Commands;

namespace Weapsy.Services.Installation
{
    public interface IThemeInstallationService
    {
        void EnsureThemeInstalled(CreateTheme createTheme);
    }
}
