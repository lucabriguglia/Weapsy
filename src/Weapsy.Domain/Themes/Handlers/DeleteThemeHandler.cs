using System;
using System.Collections.Generic;
using Weapsy.Domain.Themes.Commands;
using Weapsy.Framework.Commands;
using Weapsy.Framework.Events;

namespace Weapsy.Domain.Themes.Handlers
{
    public class DeleteThemeHandler : ICommandHandler<DeleteThemeCommand>
    {
        private readonly IThemeRepository _themeRepository;

        public DeleteThemeHandler(IThemeRepository themeRepository)
        {
            _themeRepository = themeRepository;
        }

        public IEnumerable<IEvent> Handle(DeleteThemeCommand command)
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
