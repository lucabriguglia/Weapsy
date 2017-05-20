using System.Collections.Generic;
using FluentValidation;
using Weapsy.Domain.Menus.Commands;
using System;
using Weapsy.Framework.Commands;
using Weapsy.Framework.Events;

namespace Weapsy.Domain.Menus.Handlers
{
    public class UpdateMenuItemHandler : ICommandHandler<UpdateMenuItem>
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IValidator<UpdateMenuItem> _validator;

        public UpdateMenuItemHandler(IMenuRepository menuRepository,
            IValidator<UpdateMenuItem> validator)
        {
            _menuRepository = menuRepository;
            _validator = validator;
        }

        public IEnumerable<IEvent> Handle(UpdateMenuItem command)
        {
            var menu = _menuRepository.GetById(command.SiteId, command.MenuId);

            if (menu == null)
                throw new Exception("Menu not found.");

            menu.UpdateMenuItem(command, _validator);

            _menuRepository.Update(menu);

            return menu.Events;
        }
    }
}
