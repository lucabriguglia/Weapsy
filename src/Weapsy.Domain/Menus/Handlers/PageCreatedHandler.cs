using System;
using Weapsy.Cqrs;
using Weapsy.Cqrs.Commands;
using Weapsy.Cqrs.Events;
using Weapsy.Domain.Languages;
using Weapsy.Domain.Menus.Commands;
using Weapsy.Domain.Pages.Events;

namespace Weapsy.Domain.Menus.Handlers
{
    public class PageCreatedHandler : IEventHandler<PageCreated>
    {
        private readonly IDispatcher _dispatcher;
        private readonly ILanguageRepository _languageRepository;

        public PageCreatedHandler(IDispatcher dispatcher, 
            ILanguageRepository languageRepository)
        {
            _dispatcher = dispatcher;
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

                _dispatcher.SendAndPublish<AddMenuItem, Menu>(command);
            }
        }
    }
}
