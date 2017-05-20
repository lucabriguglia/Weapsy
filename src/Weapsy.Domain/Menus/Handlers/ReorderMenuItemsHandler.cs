using System.Collections.Generic;
using System;
using Weapsy.Domain.Menus.Commands;
using FluentValidation;
using Weapsy.Framework.Commands;
using Weapsy.Framework.Events;

namespace Weapsy.Domain.Menus.Handlers
{
    public class ReorderMenuItemsHandler : ICommandHandler<ReorderMenuItems>
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IValidator<ReorderMenuItems> _validator;

        public ReorderMenuItemsHandler(IMenuRepository menuRepository, IValidator<ReorderMenuItems> validator)
        {
            _menuRepository = menuRepository;
            _validator = validator;
        }

        public IEnumerable<IEvent> Handle(ReorderMenuItems command)
        {
            var menu = _menuRepository.GetById(command.Id);

            if (menu == null)
                throw new Exception("Menu not found.");

            menu.ReorderMenuItems(command, _validator);

            _menuRepository.Update(menu);

            return menu.Events;
        }
    }
}
