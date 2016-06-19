using System.Collections.Generic;
using FluentValidation;
using Weapsy.Core.Domain;
using Weapsy.Domain.Model.Menus.Commands;

namespace Weapsy.Domain.Model.Menus.Handlers
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

        public ICollection<IEvent> Handle(CreateMenu command)
        {
            var menu = Menu.CreateNew(command, _validator);

            _menuRepository.Create(menu);

            return menu.Events;
        }
    }
}
