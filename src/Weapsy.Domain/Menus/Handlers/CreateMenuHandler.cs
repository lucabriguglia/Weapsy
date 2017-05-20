using System.Collections.Generic;
using FluentValidation;
using Weapsy.Domain.Menus.Commands;
using Weapsy.Framework.Commands;
using Weapsy.Framework.Events;

namespace Weapsy.Domain.Menus.Handlers
{
    public class CreateMenuHandler : ICommandHandler<CreateMenu>
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IValidator<CreateMenu> _validator;

        public CreateMenuHandler(IMenuRepository menuRepository,
            IValidator<CreateMenu> validator)
        {
            _menuRepository = menuRepository;
            _validator = validator;
        }

        public IEnumerable<IEvent> Handle(CreateMenu command)
        {
            var menu = Menu.CreateNew(command, _validator);

            _menuRepository.Create(menu);

            return menu.Events;
        }
    }
}
