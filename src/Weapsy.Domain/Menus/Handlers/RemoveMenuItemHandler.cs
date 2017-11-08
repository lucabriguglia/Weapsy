using System.Collections.Generic;
using System;
using Weapsy.Domain.Menus.Commands;
using FluentValidation;
using Weapsy.Framework.Commands;
using Weapsy.Framework.Events;

namespace Weapsy.Domain.Menus.Handlers
{
    public class RemoveMenuItemHandler : ICommandHandler<RemoveMenuItemCommand>
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IValidator<RemoveMenuItemCommand> _validator;

        public RemoveMenuItemHandler(IMenuRepository menuRepository, IValidator<RemoveMenuItemCommand> validator)
        {
            _menuRepository = menuRepository;
            _validator = validator;
        }

        public IEnumerable<IEvent> Handle(RemoveMenuItemCommand command)
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
