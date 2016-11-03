using System;
using System.Collections.Generic;
using Weapsy.Infrastructure.Domain;
using Weapsy.Domain.Themes.Commands;

namespace Weapsy.Domain.Themes.Handlers
{
    public class ActivateThemeHandler : ICommandHandler<ActivateTheme>
    {
        private readonly IThemeRepository _themeRepository;

        public ActivateThemeHandler(IThemeRepository themeRepository)
        {
            _themeRepository = themeRepository;
        }

        public ICollection<IEvent> Handle(ActivateTheme command)
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
