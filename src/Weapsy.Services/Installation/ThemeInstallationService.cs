using System;
using Weapsy.Cqrs;
using Weapsy.Domain.Themes;
using Weapsy.Domain.Themes.Commands;
using Weapsy.Reporting.Themes.Queries;

namespace Weapsy.Services.Installation
{
    public class ThemeInstallationService : IThemeInstallationService
    {
        private readonly IDispatcher _dispatcher;

        public ThemeInstallationService(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public void EnsureThemeInstalled(CreateTheme createTheme)
        {
            if (_dispatcher.GetResult<IsThemeInstalled, bool>(new IsThemeInstalled { Name = createTheme.Name }))
                return;

            var newThemeId = Guid.NewGuid();

            createTheme.Id = newThemeId;

            _dispatcher.SendAndPublish<CreateTheme, Theme>(createTheme);
            _dispatcher.SendAndPublish<ActivateTheme, Theme>(new ActivateTheme { Id = newThemeId });
        }
    }
}
