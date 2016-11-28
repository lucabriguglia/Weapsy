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
    public class CreateMenuHandlerTests
    {
        [Test]
        public void Should_throw_validation_exception_when_validation_fails()
        {
            var command = new CreateMenu
            {
                SiteId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Name = "My Menu"
            };

            var menuRepositoryMock = new Mock<IMenuRepository>();

            var validatorMock = new Mock<IValidator<CreateMenu>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Id", "Id Error") }));

            var createMenuHandler = new CreateMenuHandler(menuRepositoryMock.Object, validatorMock.Object);

            Assert.Throws<Exception>(() => createMenuHandler.Handle(command));
        }

        [Test]
        public void Should_validate_command_and_save_new_menu()
        {
            var command = new CreateMenu
            {
                SiteId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Name = "My Menu"
            };

            var menuRepositoryMock = new Mock<IMenuRepository>();
            menuRepositoryMock.Setup(x => x.Create(It.IsAny<Menu>()));

            var validatorMock = new Mock<IValidator<CreateMenu>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            var createMenuHandler = new CreateMenuHandler(menuRepositoryMock.Object, validatorMock.Object);
            createMenuHandler.Handle(command);

            validatorMock.Verify(x => x.Validate(command));
            menuRepositoryMock.Verify(x => x.Create(It.IsAny<Menu>()));
        }
    }
}
