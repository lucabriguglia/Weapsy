using System.Collections.Generic;
using FluentValidation;
using Weapsy.Core.Domain;
using Weapsy.Domain.Model.Themes.Commands;

namespace Weapsy.Domain.Model.Themes.Handlers
{
    public class CreateThemeHandler : ICommandHandler<CreateTheme>
    {
        private readonly IThemeRepository _themeRepository;
        private readonly IValidator<CreateTheme> _validator;
        private readonly IThemeSortOrderGenerator _sortOrderGenerator;

        public CreateThemeHandler(IThemeRepository themeRepository,
            IValidator<CreateTheme> validator,
            IThemeSortOrderGenerator sortOrderGenerator)
        {
            _themeRepository = themeRepository;
            _validator = validator;
            _sortOrderGenerator = sortOrderGenerator;
        }

        public ICollection<IEvent> Handle(CreateTheme command)
        {
            var theme = Theme.CreateNew(command, _validator, _sortOrderGenerator);

            _themeRepository.Create(theme);

            return theme.Events;
        }
    }
}
