using Weapsy.Domain.Themes.Commands;
using Weapsy.Infrastructure.Domain;

namespace Weapsy.Services.Installation
{
    public interface IThemeInstallationService : IService
    {
        void EnsureThemeInstalled(CreateTheme createTheme);
    }
}
