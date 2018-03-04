using Weapsy.Domain.Menus.Commands;
using System;
using Weapsy.Cqrs.Commands;
using Weapsy.Cqrs.Domain;

namespace Weapsy.Domain.Menus.Handlers
{
    public class SetMenuItemPermissionsHandler : ICommandHandlerWithAggregate<SetMenuItemPermissions>
    {
        private readonly IMenuRepository _menuRepository;

        public SetMenuItemPermissionsHandler(IMenuRepository menuRepository)
        {
            _menuRepository = menuRepository;
        }

        public IAggregateRoot Handle(SetMenuItemPermissions cmd)
        {
            var menu = _menuRepository.GetById(cmd.SiteId, cmd.MenuId);

            if (menu == null)
                throw new Exception("Menu not found");

            menu.SetMenuItemPermissions(cmd);

            _menuRepository.Update(menu);

            return menu;
        }
    }
}
