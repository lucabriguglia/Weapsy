using System;
using Weapsy.Domain.Themes;
using Weapsy.Domain.Themes.Commands;
using Weapsy.Framework.Commands;
using Weapsy.Framework.Queries;
using Weapsy.Reporting.Themes.Queries;

namespace Weapsy.Services.Installation
{
    public class ThemeInstallationService : IThemeInstallationService
    {
        private readonly ICommandSender _commandSender;
        private readonly IQueryDispatcher _queryDispatcher;

        public ThemeInstallationService(ICommandSender commandSender,
            IQueryDispatcher queryDispatcher)
        {
            _commandSender = commandSender;
            _queryDispatcher = queryDispatcher;
        }

        public void EnsureThemeInstalled(CreateThemeCommand createTheme)
        {
            if (_queryDispatcher.Dispatch<IsThemeInstalled, bool>(new IsThemeInstalled { Name = createTheme.Name }))
                return;

            var newThemeId = Guid.NewGuid();

            createTheme.Id = newThemeId;

            _commandSender.Send<CreateThemeCommand, Theme>(createTheme, false);
            _commandSender.Send<ActivateThemeCommand, Theme>(new ActivateThemeCommand { Id = newThemeId }, false);
        }
    }
}
