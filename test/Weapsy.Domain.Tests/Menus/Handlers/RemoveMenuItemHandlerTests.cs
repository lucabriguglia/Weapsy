using System;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Menus;
using Weapsy.Domain.Menus.Commands;
using FluentValidation;
using FluentValidation.Results;
using Weapsy.Domain.Menus.Handlers;
using System.Collections.Generic;
using System.Linq;

namespace Weapsy.Domain.Tests.Menus.Handlers
{
    [TestFixture]
    public class RemoveMenuItemHandlerTests
    {
        [Test]
        public void Should_throw_exception_if_menu_not_found()
        {
            var command = new RemoveMenuItemCommand
            {
                MenuId = Guid.NewGuid()
            };

            var menuRepositoryMock = new Mock<IMenuRepository>();
            menuRepositoryMock.Setup(x => x.GetById(command.MenuId)).Returns((Menu)null);

            var validatorMock = new Mock<IValidator<RemoveMenuItemCommand>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            var handler = new RemoveMenuItemHandler(menuRepositoryMock.Object, validatorMock.Object);

            Assert.Throws<Exception>(() => handler.Handle(command));
        }

        [Test]
        public void Should_update_menu_when_remove_menu_item()
        {
            var createMenuCommand = new CreateMenuCommand
            {
                SiteId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Name = "My Menu"
            };
            var createMenuValidatorMock = new Mock<IValidator<CreateMenuCommand>>();
            createMenuValidatorMock.Setup(x => x.Validate(createMenuCommand)).Returns(new ValidationResult());
            var menu = Menu.CreateNew(createMenuCommand, createMenuValidatorMock.Object);

            var addMenuItemCommand = new AddMenuItemCommand
            {
                SiteId = menu.SiteId,
                MenuId = menu.Id,
                MenuItemId = Guid.NewGuid(),
                Type = MenuItemType.Link,
                PageId = Guid.NewGuid(),
                Link = "link",
                Text = "Text",
                Title = "Title",
                MenuItemLocalisations = new List<MenuItemLocalisation>
                {
                    new MenuItemLocalisation
                    {
                        LanguageId = Guid.NewGuid(),
                        Text = "Text 1",
                        Title = "Title 1"
                    },
                    new MenuItemLocalisation
                    {
                        LanguageId = Guid.NewGuid(),
                        Text = "Text 2",
                        Title = "Title 2"
                    }
                }
            };
            var addMenuItemValidatorMock = new Mock<IValidator<AddMenuItemCommand>>();
            addMenuItemValidatorMock.Setup(x => x.Validate(addMenuItemCommand)).Returns(new ValidationResult());
            menu.AddMenuItem(addMenuItemCommand, addMenuItemValidatorMock.Object);

            var command = new RemoveMenuItemCommand
            {
                MenuId = menu.Id,
                MenuItemId = menu.MenuItems.FirstOrDefault().Id
            };

            var menuRepositoryMock = new Mock<IMenuRepository>();
            menuRepositoryMock.Setup(x => x.GetById(It.IsAny<Guid>())).Returns(menu);
            menuRepositoryMock.Setup(x => x.Update(It.IsAny<Menu>()));

            var validatorMock = new Mock<IValidator<RemoveMenuItemCommand>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            var createMenuHandler = new RemoveMenuItemHandler(menuRepositoryMock.Object, validatorMock.Object);
            createMenuHandler.Handle(command);

            menuRepositoryMock.Verify(x => x.Update(It.IsAny<Menu>()));
        }
    }
}
