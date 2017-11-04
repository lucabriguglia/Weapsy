using System.Collections.Generic;
using FluentValidation;
using Weapsy.Domain.Menus.Commands;
using Weapsy.Framework.Commands;
using Weapsy.Framework.Events;

namespace Weapsy.Domain.Menus.Handlers
{
    public class CreateMenuHandler : ICommandHandler<CreateMenuCommand>
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IValidator<CreateMenuCommand> _validator;

        public CreateMenuHandler(IMenuRepository menuRepository,
            IValidator<CreateMenuCommand> validator)
        {
            _menuRepository = menuRepository;
            _validator = validator;
        }

        public IEnumerable<IEvent> Handle(CreateMenuCommand command)
        {
            var menu = Menu.CreateNew(command, _validator);

            _menuRepository.Create(menu);

            return menu.Events;
        }
    }
}
