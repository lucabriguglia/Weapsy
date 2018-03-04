using System;
using Weapsy.Cqrs.Commands;
using Weapsy.Cqrs.Domain;
using Weapsy.Domain.Themes.Commands;

namespace Weapsy.Domain.Themes.Handlers
{
    public class DeleteThemeHandler : ICommandHandlerWithAggregate<DeleteTheme>
    {
        private readonly IThemeRepository _themeRepository;

        public DeleteThemeHandler(IThemeRepository themeRepository)
        {
            _themeRepository = themeRepository;
        }

        public IAggregateRoot Handle(DeleteTheme command)
        {
            var theme = _themeRepository.GetById(command.Id);

            if (theme == null)
                throw new Exception("Theme not found.");

            theme.Delete();

            _themeRepository.Update(theme);

            return theme;
        }
    }
}
