using FluentValidation;
using Weapsy.Cqrs.Commands;
using Weapsy.Cqrs.Domain;
using Weapsy.Domain.Menus.Commands;

namespace Weapsy.Domain.Menus.Handlers
{
    public class CreateMenuHandler : ICommandHandlerWithAggregate<CreateMenu>
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IValidator<CreateMenu> _validator;

        public CreateMenuHandler(IMenuRepository menuRepository,
            IValidator<CreateMenu> validator)
        {
            _menuRepository = menuRepository;
            _validator = validator;
        }

        public IAggregateRoot Handle(CreateMenu command)
        {
            var menu = Menu.CreateNew(command, _validator);

            _menuRepository.Create(menu);

            return menu;
        }
    }
}
