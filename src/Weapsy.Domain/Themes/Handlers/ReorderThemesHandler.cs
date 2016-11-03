using System.Collections.Generic;
using Weapsy.Infrastructure.Domain;
using Weapsy.Domain.Themes.Commands;
using System;

namespace Weapsy.Domain.Themes.Handlers
{
    public class ReorderThemesHandler : ICommandHandler<ReorderThemes>
    {
        private readonly IThemeRepository _themeRepository;

        public ReorderThemesHandler(IThemeRepository themeRepository)
        {
            _themeRepository = themeRepository;
        }

        public ICollection<IEvent> Handle(ReorderThemes cmd)
        {
            var events = new List<IEvent>();
            var updatedThemes = new List<Theme>();

            for (int i = 0; i < cmd.Themes.Count; i++)
            {
                var themeId = cmd.Themes[i];
                var sortOrder = i + 1;

                var theme = _themeRepository.GetById(themeId);

                if (theme == null)
                    throw new Exception("Theme not found.");

                if (theme.SortOrder != sortOrder)
                {
                    theme.Reorder(sortOrder);
                    updatedThemes.Add(theme);
                    events.AddRange(theme.Events);
                }
            }

            _themeRepository.Update(updatedThemes);

            return events;
        }
    }
}
