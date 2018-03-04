using FluentValidation;
using Weapsy.Domain.Menus.Commands;
using System;
using Weapsy.Cqrs.Commands;
using Weapsy.Cqrs.Domain;

namespace Weapsy.Domain.Menus.Handlers
{
    public class UpdateMenuItemHandler : ICommandHandlerWithAggregate<UpdateMenuItem>
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IValidator<UpdateMenuItem> _validator;

        public UpdateMenuItemHandler(IMenuRepository menuRepository,
            IValidator<UpdateMenuItem> validator)
        {
            _menuRepository = menuRepository;
            _validator = validator;
        }

        public IAggregateRoot Handle(UpdateMenuItem command)
        {
            var menu = _menuRepository.GetById(command.SiteId, command.MenuId);

            if (menu == null)
                throw new Exception("Menu not found.");

            menu.UpdateMenuItem(command, _validator);

            _menuRepository.Update(menu);

            return menu;
        }
    }
}
