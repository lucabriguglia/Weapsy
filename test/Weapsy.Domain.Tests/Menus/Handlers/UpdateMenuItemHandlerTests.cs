using System;
using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Menus;
using Weapsy.Domain.Menus.Commands;
using Weapsy.Domain.Menus.Handlers;

namespace Weapsy.Domain.Tests.Menus.Handlers
{
    [TestFixture]
    public class UpdateMenuItemHandlerTests
    {
        [Test]
        public void Should_throw_exception_if_menu_not_found()
        {
            var command = new UpdateMenuItem
            {
                MenuId = Guid.NewGuid()
            };

            var menuRepositoryMock = new Mock<IMenuRepository>();
            menuRepositoryMock.Setup(x => x.GetById(command.MenuId)).Returns((Menu)null);

            var validatorMock = new Mock<IValidator<UpdateMenuItem>>();

            var handler = new UpdateMenuItemHandler(menuRepositoryMock.Object, validatorMock.Object);

            Assert.Throws<Exception>(() => handler.Handle(command));
        }

        [Test]
        public void Should_throw_exception_when_validation_fails()
        {
            var command = new UpdateMenuItem
            {
                SiteId = Guid.NewGuid(),
                MenuId = Guid.NewGuid(),
                MenuItemId = Guid.NewGuid(),
                MenuItemLocalisations = new List<MenuItemLocalisation>()
            };

            var menuRepositoryMock = new Mock<IMenuRepository>();
            menuRepositoryMock.Setup(x => x.GetById(It.IsAny<Guid>())).Returns(new Menu());

            var validatorMock = new Mock<IValidator<UpdateMenuItem>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Id", "Id Error") }));

            var createMenuHandler = new UpdateMenuItemHandler(menuRepositoryMock.Object, validatorMock.Object);

            Assert.Throws<Exception>(() => createMenuHandler.Handle(command));
        }

        [Test]
        public void Should_update_menu_when_add_menu_item()
        {
            var menu = new Menu();

            var menuItemId = Guid.NewGuid();

            var addMenuItemCommand = new AddMenuItem
            {
                SiteId = Guid.NewGuid(),
                MenuId = Guid.NewGuid(),
                MenuItemId = menuItemId,
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

            var addMenuItemValidatorMock = new Mock<IValidator<AddMenuItem>>();
            addMenuItemValidatorMock.Setup(x => x.Validate(addMenuItemCommand)).Returns(new ValidationResult());

            menu.AddMenuItem(addMenuItemCommand, addMenuItemValidatorMock.Object);

            var command = new UpdateMenuItem
            {
                SiteId = Guid.NewGuid(),
                MenuId = Guid.NewGuid(),
                MenuItemId = menuItemId,
                MenuItemLocalisations = new List<MenuItemLocalisation>()
            };

            var menuRepositoryMock = new Mock<IMenuRepository>();
            menuRepositoryMock.Setup(x => x.GetById(command.SiteId, command.MenuId)).Returns(menu);
            menuRepositoryMock.Setup(x => x.Update(menu));

            var validatorMock = new Mock<IValidator<UpdateMenuItem>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            var createMenuHandler = new UpdateMenuItemHandler(menuRepositoryMock.Object, validatorMock.Object);
            createMenuHandler.Handle(command);

            menuRepositoryMock.Verify(x => x.Update(menu));
        }
    }
}
