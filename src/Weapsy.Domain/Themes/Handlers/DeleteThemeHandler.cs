using System;
using System.Collections.Generic;
using Weapsy.Core.Domain;
using Weapsy.Domain.Model.Themes.Commands;

namespace Weapsy.Domain.Model.Themes.Handlers
{
    public class DeleteThemeHandler : ICommandHandler<DeleteTheme>
    {
        private readonly IThemeRepository _themeRepository;

        public DeleteThemeHandler(IThemeRepository themeRepository)
        {
            _themeRepository = themeRepository;
        }

        public ICollection<IEvent> Handle(DeleteTheme command)
        {
            var theme = _themeRepository.GetById(command.Id);

            if (theme == null)
                throw new Exception("Theme not found.");

            theme.Delete();

            _themeRepository.Update(theme);

            return theme.Events;
        }
    }
}
