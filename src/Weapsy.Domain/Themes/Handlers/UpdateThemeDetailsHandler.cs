using FluentValidation;
using Weapsy.Domain.Themes.Commands;
using System;
using Weapsy.Cqrs.Commands;
using Weapsy.Cqrs.Domain;

namespace Weapsy.Domain.Themes.Handlers
{
    public class UpdateThemeDetailsHandler : ICommandHandlerWithAggregate<UpdateThemeDetails>
    {
        private readonly IThemeRepository _themeRepository;
        private readonly IValidator<UpdateThemeDetails> _validator;

        public UpdateThemeDetailsHandler(IThemeRepository themeRepository, IValidator<UpdateThemeDetails> validator)
        {
            _themeRepository = themeRepository;
            _validator = validator;
        }

        public IAggregateRoot Handle(UpdateThemeDetails cmd)
        {
            var theme = _themeRepository.GetById(cmd.Id);

            if (theme == null)
                throw new Exception("Theme not found.");

            theme.UpdateDetails(cmd, _validator);

            _themeRepository.Update(theme);

            return theme;
        }
    }
}
