using System.Collections.Generic;
using System;
using Weapsy.Domain.Menus.Commands;
using FluentValidation;
using Weapsy.Framework.Commands;
using Weapsy.Framework.Events;

namespace Weapsy.Domain.Menus.Handlers
{
    public class RemoveMenuItemHandler : ICommandHandler<RemoveMenuItem>
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IValidator<RemoveMenuItem> _validator;

        public RemoveMenuItemHandler(IMenuRepository menuRepository, IValidator<RemoveMenuItem> validator)
        {
            _menuRepository = menuRepository;
            _validator = validator;
        }

        public IEnumerable<IEvent> Handle(RemoveMenuItem command)
        {
            var menu = _menuRepository.GetById(command.MenuId);

            if (menu == null)
                throw new Exception("Menu not found.");

            menu.RemoveMenuItem(command, _validator);

            _menuRepository.Update(menu);

            return menu.Events;
        }
    }
}
