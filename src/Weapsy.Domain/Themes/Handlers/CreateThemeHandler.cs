using System.Collections.Generic;
using FluentValidation;
using Weapsy.Domain.Themes.Commands;
using Weapsy.Framework.Commands;
using Weapsy.Framework.Events;

namespace Weapsy.Domain.Themes.Handlers
{
    public class CreateThemeHandler : ICommandHandler<CreateThemeCommand>
    {
        private readonly IThemeRepository _themeRepository;
        private readonly IValidator<CreateThemeCommand> _validator;
        private readonly IThemeSortOrderGenerator _sortOrderGenerator;

        public CreateThemeHandler(IThemeRepository themeRepository,
            IValidator<CreateThemeCommand> validator,
            IThemeSortOrderGenerator sortOrderGenerator)
        {
            _themeRepository = themeRepository;
            _validator = validator;
            _sortOrderGenerator = sortOrderGenerator;
        }

        public IEnumerable<IEvent> Handle(CreateThemeCommand command)
        {
            var theme = Theme.CreateNew(command, _validator, _sortOrderGenerator);

            _themeRepository.Create(theme);

            return theme.Events;
        }
    }
}
