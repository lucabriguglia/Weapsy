using System.Collections.Generic;
using Weapsy.Core.Domain;
using System;
using Weapsy.Domain.Model.Menus.Commands;
using FluentValidation;

namespace Weapsy.Domain.Model.Menus.Handlers
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

        public ICollection<IEvent> Handle(RemoveMenuItem command)
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
