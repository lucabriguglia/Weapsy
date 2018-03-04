using System;
using Weapsy.Domain.Menus.Commands;
using FluentValidation;
using Weapsy.Cqrs.Commands;
using Weapsy.Cqrs.Domain;

namespace Weapsy.Domain.Menus.Handlers
{
    public class ReorderMenuItemsHandler : ICommandHandlerWithAggregate<ReorderMenuItems>
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IValidator<ReorderMenuItems> _validator;

        public ReorderMenuItemsHandler(IMenuRepository menuRepository, IValidator<ReorderMenuItems> validator)
        {
            _menuRepository = menuRepository;
            _validator = validator;
        }

        public IAggregateRoot Handle(ReorderMenuItems command)
        {
            var menu = _menuRepository.GetById(command.Id);

            if (menu == null)
                throw new Exception("Menu not found.");

            menu.ReorderMenuItems(command, _validator);

            _menuRepository.Update(menu);

            return menu;
        }
    }
}
