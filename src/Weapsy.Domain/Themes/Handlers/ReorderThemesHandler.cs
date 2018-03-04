using Weapsy.Domain.Themes.Commands;
using System;
using Weapsy.Cqrs.Commands;
using Weapsy.Cqrs.Domain;

namespace Weapsy.Domain.Themes.Handlers
{
    public class ReorderThemesHandler : ICommandHandlerWithAggregate<ReorderTheme>
    {
        private readonly IThemeRepository _themeRepository;

        public ReorderThemesHandler(IThemeRepository themeRepository)
        {
            _themeRepository = themeRepository;
        }

        public IAggregateRoot Handle(ReorderTheme cmd)
        {
            var language = _themeRepository.GetById(cmd.AggregateRootId);

            if (language == null)
                throw new Exception("Theme not found.");

            language.Reorder(cmd.Order);
            _themeRepository.Update(language);

            return language;
        }
    }
}
