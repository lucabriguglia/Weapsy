using System;
using System.Collections.Generic;
using Weapsy.Domain.Themes.Commands;
using Weapsy.Framework.Commands;
using Weapsy.Framework.Events;

namespace Weapsy.Domain.Themes.Handlers
{
    public class ActivateThemeHandler : ICommandHandler<ActivateTheme>
    {
        private readonly IThemeRepository _themeRepository;

        public ActivateThemeHandler(IThemeRepository themeRepository)
        {
            _themeRepository = themeRepository;
        }

        public IEnumerable<IEvent> Handle(ActivateTheme command)
        {
            var theme = _themeRepository.GetById(command.Id);

            if (theme == null)
                throw new Exception("Theme not found.");

            theme.Activate();

            _themeRepository.Update(theme);

            return theme.Events;
        }
    }
}
