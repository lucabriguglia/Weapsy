using System.Linq;
using NUnit.Framework;
using Weapsy.Domain.Menus;
using Weapsy.Domain.Menus.Events;
using System;
using Weapsy.Domain.Menus.Commands;
using Moq;
using FluentValidation;
using FluentValidation.Results;

namespace Weapsy.Domain.Tests.Menus
{
    [TestFixture]
    public class DeleteMenuTests
    {
        [Test]
        public void Should_throw_exception_when_already_deleted()
        {
            var command = new CreateMenu
            {
                SiteId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Name = "My Menu"
            };
            var validatorMock = new Mock<IValidator<CreateMenu>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());
            var menu = Menu.CreateNew(command, validatorMock.Object);

            menu.Delete();
            
            Assert.Throws<Exception>(() => menu.Delete());
        }

        [Test]
        public void Should_throw_exception_when_deleting_main_menu()
        {
            var command = new CreateMenu
            {
                SiteId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Name = "Main"
            };
            var validatorMock = new Mock<IValidator<CreateMenu>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());
            var menu = Menu.CreateNew(command, validatorMock.Object);

            Assert.Throws<Exception>(() => menu.Delete());
        }

        [Test]
        public void Should_set_menu_status_to_deleted()
        {
            var command = new CreateMenu
            {
                SiteId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Name = "My Menu"
            };
            var validatorMock = new Mock<IValidator<CreateMenu>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());
            var menu = Menu.CreateNew(command, validatorMock.Object);

            menu.Delete();

            Assert.AreEqual(true, menu.Status == MenuStatus.Deleted);
        }

        [Test]
        public void Should_add_menu_deleted_event()
        {
            var command = new CreateMenu
            {
                SiteId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Name = "My Menu"
            };
            var validatorMock = new Mock<IValidator<CreateMenu>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());
            var menu = Menu.CreateNew(command, validatorMock.Object);

            menu.Delete();

            var @event = menu.Events.OfType<MenuDeleted>().SingleOrDefault();

            Assert.IsNotNull(@event);
        }

        [Test]
        public void Should_set_id_in_menu_deleted_event()
        {
            var command = new CreateMenu
            {
                SiteId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Name = "My Menu"
            };
            var validatorMock = new Mock<IValidator<CreateMenu>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());
            var menu = Menu.CreateNew(command, validatorMock.Object);

            menu.Delete();

            var @event = menu.Events.OfType<MenuDeleted>().SingleOrDefault();

            Assert.AreEqual(menu.Id, @event.AggregateRootId);
        }

        [Test]
        public void Should_set_site_id_in_menu_deleted_event()
        {
            var command = new CreateMenu
            {
                SiteId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Name = "My Menu"
            };
            var validatorMock = new Mock<IValidator<CreateMenu>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());
            var menu = Menu.CreateNew(command, validatorMock.Object);

            menu.Delete();

            var @event = menu.Events.OfType<MenuDeleted>().SingleOrDefault();

            Assert.AreEqual(menu.SiteId, @event.SiteId);
        }
    }
}
