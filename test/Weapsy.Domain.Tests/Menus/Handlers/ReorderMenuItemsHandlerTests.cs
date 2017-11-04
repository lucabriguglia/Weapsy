using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Menus;
using Weapsy.Domain.Menus.Commands;
using Weapsy.Domain.Menus.Handlers;
using FluentValidation;
using FluentValidation.Results;

namespace Weapsy.Domain.Tests.Menus.Handlers
{
    [TestFixture]
    public class ReorderMenuItemsHandlerTests
    {
        [Test]
        public void Should_throw_exception_if_menu_not_found()
        {
            var command = new ReorderMenuItemsCommand
            {
                Id = Guid.NewGuid(),
                MenuItems = new List<ReorderMenuItemsCommand.MenuItem>()
            };

            var menuRepositoryMock = new Mock<IMenuRepository>();
            menuRepositoryMock.Setup(x => x.GetById(command.Id)).Returns((Menu)null);

            var validatorMock = new Mock<IValidator<ReorderMenuItemsCommand>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            var handler = new ReorderMenuItemsHandler(menuRepositoryMock.Object, validatorMock.Object);

            Assert.Throws<Exception>(() => handler.Handle(command));
        }

        [Test]
        public void Should_save_menu_when_reorder_menu_items()
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

            var reorderMenuItemsCommand = new ReorderMenuItemsCommand
            {
                Id = Guid.NewGuid(),
                MenuItems = new List<ReorderMenuItemsCommand.MenuItem>()
            };

            var menuRepositoryMock = new Mock<IMenuRepository>();
            menuRepositoryMock.Setup(x => x.GetById(It.IsAny<Guid>())).Returns(menu);
            menuRepositoryMock.Setup(x => x.Update(It.IsAny<Menu>()));

            var validatorMock = new Mock<IValidator<ReorderMenuItemsCommand>>();
            validatorMock.Setup(x => x.Validate(reorderMenuItemsCommand)).Returns(new ValidationResult());

            var createMenuHandler = new ReorderMenuItemsHandler(menuRepositoryMock.Object, validatorMock.Object);
            createMenuHandler.Handle(reorderMenuItemsCommand);

            menuRepositoryMock.Verify(x => x.Update(It.IsAny<Menu>()));
        }
    }
}
