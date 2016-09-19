using System;
using System.Linq;
using NUnit.Framework;
using Moq;
using FluentValidation;
using FluentValidation.Results;
using Weapsy.Domain.Menus;
using Weapsy.Domain.Menus.Commands;
using Weapsy.Domain.Menus.Events;

namespace Weapsy.Domain.Tests.Menus
{
    [TestFixture]
    public class CreateMenuTests
    {
        private CreateMenu _command;
        private Mock<IValidator<CreateMenu>> _validatorMock;
        private Menu _menu;
        private MenuCreated _event;

        [SetUp]
        public void Setup()
        {
            _command = new CreateMenu
            {
                SiteId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Name = "My Menu"
            };
            _validatorMock = new Mock<IValidator<CreateMenu>>();
            _validatorMock.Setup(x => x.Validate(_command)).Returns(new ValidationResult());
            _menu = Menu.CreateNew(_command, _validatorMock.Object);
            _event = _menu.Events.OfType<MenuCreated>().SingleOrDefault();
        }

        [Test]
        public void Should_validate_command()
        {
            _validatorMock.Verify(x => x.Validate(_command));
        }

        [Test]
        public void Should_set_id()
        {
            Assert.AreEqual(_command.Id, _menu.Id);
        }

        [Test]
        public void Should_set_site_id()
        {
            Assert.AreEqual(_command.SiteId, _menu.SiteId);
        }

        [Test]
        public void Should_set_name()
        {
            Assert.AreEqual(_command.Name, _menu.Name);
        }

        public void Should_set_status_to_active()
        {
            Assert.AreEqual(MenuStatus.Active, _menu.Status);
        }

        [Test]
        public void Should_add_menu_created_event()
        {
            Assert.IsNotNull(_event);
        }

        [Test]
        public void Should_set_id_in_menu_created_event()
        {
            Assert.AreEqual(_menu.Id, _event.AggregateRootId);
        }

        [Test]
        public void Should_set_site_id_in_menu_created_event()
        {
            Assert.AreEqual(_menu.SiteId, _event.SiteId);
        }

        [Test]
        public void Should_set_name_in_menu_created_event()
        {
            Assert.AreEqual(_menu.Name, _event.Name);
        }

        [Test]
        public void Should_set_status_in_menu_created_event()
        {
            Assert.AreEqual(_menu.Status, _event.Status);
        }
    }
}
