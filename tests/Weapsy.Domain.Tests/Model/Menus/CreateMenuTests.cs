using System;
using System.Linq;
using NUnit.Framework;
using Moq;
using FluentValidation;
using FluentValidation.Results;
using Weapsy.Domain.Model.Menus;
using Weapsy.Domain.Model.Menus.Commands;
using Weapsy.Domain.Model.Menus.Events;

namespace Weapsy.Domain.Tests.Menus
{
    [TestFixture]
    public class CreateMenuTests
    {
        private CreateMenu command;
        private Mock<IValidator<CreateMenu>> validatorMock;
        private Menu menu;
        private MenuCreated @event;

        [SetUp]
        public void Setup()
        {
            command = new CreateMenu
            {
                SiteId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Name = "My Menu"
            };
            validatorMock = new Mock<IValidator<CreateMenu>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());
            menu = Menu.CreateNew(command, validatorMock.Object);
            @event = menu.Events.OfType<MenuCreated>().SingleOrDefault();
        }

        [Test]
        public void Should_validate_command()
        {
            validatorMock.Verify(x => x.Validate(command));
        }

        [Test]
        public void Should_set_id()
        {
            Assert.AreEqual(command.Id, menu.Id);
        }

        [Test]
        public void Should_set_site_id()
        {
            Assert.AreEqual(command.SiteId, menu.SiteId);
        }

        [Test]
        public void Should_set_name()
        {
            Assert.AreEqual(command.Name, menu.Name);
        }

        public void Should_set_status_to_active()
        {
            Assert.AreEqual(MenuStatus.Active, menu.Status);
        }

        [Test]
        public void Should_add_menu_created_event()
        {
            Assert.IsNotNull(@event);
        }

        [Test]
        public void Should_set_id_in_menu_created_event()
        {
            Assert.AreEqual(menu.Id, @event.AggregateRootId);
        }

        [Test]
        public void Should_set_site_id_in_menu_created_event()
        {
            Assert.AreEqual(menu.SiteId, @event.SiteId);
        }

        [Test]
        public void Should_set_name_in_menu_created_event()
        {
            Assert.AreEqual(menu.Name, @event.Name);
        }

        [Test]
        public void Should_set_status_in_menu_created_event()
        {
            Assert.AreEqual(menu.Status, @event.Status);
        }
    }
}
