using Weapsy.Domain.Themes.Commands;
using Weapsy.Framework.Domain;

namespace Weapsy.Services.Installation
{
    public interface IThemeInstallationService : IService
    {
        void EnsureThemeInstalled(CreateTheme createTheme);
    }
}
