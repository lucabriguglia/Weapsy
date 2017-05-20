using System;
using Weapsy.Domain.Languages;
using Weapsy.Domain.Menus.Commands;
using Weapsy.Domain.Pages.Events;
using Weapsy.Framework.Commands;
using Weapsy.Framework.Events;

namespace Weapsy.Domain.Menus.Handlers
{
    public class PageCreatedHandler : IEventHandler<PageCreated>
    {
        private readonly ICommandSender _commandSender;
        private readonly ILanguageRepository _languageRepository;

        public PageCreatedHandler(ICommandSender commandSender, 
            ILanguageRepository languageRepository)
        {
            _commandSender = commandSender;
            _languageRepository = languageRepository;
        }

        public void Handle(PageCreated @event)
        {
            foreach (var menuId in @event.MenuIds)
            {
                var command = new AddMenuItem
                {
                    SiteId = @event.SiteId,
                    MenuId = menuId,
                    MenuItemId = Guid.NewGuid(),
                    Type = MenuItemType.Page,
                    PageId = @event.AggregateRootId,
                    Text = !string.IsNullOrEmpty(@event.Title) ? @event.Title : @event.Name
                };

                foreach (var languageId in _languageRepository.GetLanguagesIdList(@event.SiteId))
                {
                    command.MenuItemLocalisations.Add(new MenuItemLocalisation
                    {
                        LanguageId = languageId
                    });
                }

                _commandSender.Send<AddMenuItem, Menu>(command);
            }
        }
    }
}
