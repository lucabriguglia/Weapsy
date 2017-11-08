using System.Collections.Generic;
using System;
using Weapsy.Domain.Menus.Commands;
using FluentValidation;
using Weapsy.Framework.Commands;
using Weapsy.Framework.Events;

namespace Weapsy.Domain.Menus.Handlers
{
    public class ReorderMenuItemsHandler : ICommandHandler<ReorderMenuItemsCommand>
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IValidator<ReorderMenuItemsCommand> _validator;

        public ReorderMenuItemsHandler(IMenuRepository menuRepository, IValidator<ReorderMenuItemsCommand> validator)
        {
            _menuRepository = menuRepository;
            _validator = validator;
        }

        public IEnumerable<IEvent> Handle(ReorderMenuItemsCommand command)
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
