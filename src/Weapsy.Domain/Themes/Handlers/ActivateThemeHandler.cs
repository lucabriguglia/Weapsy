using System;
using Weapsy.Cqrs.Commands;
using Weapsy.Cqrs.Domain;
using Weapsy.Domain.Themes.Commands;

namespace Weapsy.Domain.Themes.Handlers
{
    public class ActivateThemeHandler : ICommandHandlerWithAggregate<ActivateTheme>
    {
        private readonly IThemeRepository _themeRepository;

        public ActivateThemeHandler(IThemeRepository themeRepository)
        {
            _themeRepository = themeRepository;
        }

        public IAggregateRoot Handle(ActivateTheme command)
        {
            var theme = _themeRepository.GetById(command.Id);

            if (theme == null)
                throw new Exception("Theme not found.");

            theme.Activate();

            _themeRepository.Update(theme);

            return theme;
        }
    }
}
