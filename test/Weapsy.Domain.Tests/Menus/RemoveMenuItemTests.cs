using System;
using System.Linq;
using NUnit.Framework;
using Moq;
using FluentValidation;
using FluentValidation.Results;
using Weapsy.Domain.Menus;
using Weapsy.Domain.Menus.Commands;
using System.Collections.Generic;
using Weapsy.Domain.Menus.Events;
using System.Reflection;

namespace Weapsy.Domain.Tests.Menus
{
    [TestFixture]
    public class RemoveMenuItemTests
    {
        private Menu _menu;
        private RemoveMenuItem _command;
        private MenuItemRemoved _event;
        private Mock<IValidator<RemoveMenuItem>> _validatorMock;

        [SetUp]
        public void Setup()
        {
            _menu = new Menu();

            var addMenuItemCommand = new AddMenuItem
            {
                SiteId = Guid.NewGuid(),
                MenuId = Guid.NewGuid(),
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

            var addMenuItemValidatorMock = new Mock<IValidator<AddMenuItem>>();
            addMenuItemValidatorMock.Setup(x => x.Validate(addMenuItemCommand)).Returns(new ValidationResult());

            _menu.AddMenuItem(addMenuItemCommand, addMenuItemValidatorMock.Object);

            _command = new RemoveMenuItem
            {
                MenuId = _menu.Id,
                MenuItemId = addMenuItemCommand.MenuItemId
            };

            _validatorMock = new Mock<IValidator<RemoveMenuItem>>();
            _validatorMock.Setup(x => x.Validate(_command)).Returns(new ValidationResult());

            _menu.RemoveMenuItem(_command, _validatorMock.Object);

            _event = _menu.Events.OfType<MenuItemRemoved>().SingleOrDefault();
        }

        [Test]
        public void Should_remove_menu_item()
        {
            Assert.AreEqual(MenuItemStatus.Deleted, _menu.MenuItems.FirstOrDefault(x => x.Id == _command.MenuItemId).Status);
        }

        [Test]
        public void Should_add_menu_item_removed_event()
        {
            Assert.IsNotNull(_event);
        }

        [Test]
        public void Should_set_id_in_menu_item_removed_event()
        {
            Assert.AreEqual(_menu.Id, _event.AggregateRootId);
        }

        [Test]
        public void Should_set_site_id_in_menu_item_removed_event()
        {
            Assert.AreEqual(_menu.SiteId, _event.SiteId);
        }

        [Test]
        public void Should_set_menu_item_in_menu_item_removed_event()
        {
            Assert.AreEqual(_command.MenuItemId, _event.MenuItemId);
        }

        [Test]
        public void Should_throw_exception_if_menu_item_does_not_exist()
        {
            _command = new RemoveMenuItem
            {
                MenuId = _menu.Id,
                MenuItemId = Guid.NewGuid()
            };
            _validatorMock.Setup(x => x.Validate(_command)).Returns(new ValidationResult());
            Assert.Throws<Exception>(() => _menu.RemoveMenuItem(_command, _validatorMock.Object));
        }

        [Test]
        public void Should_throw_exception_if_menu_item_is_deleted()
        {
            var menuItem = _menu.MenuItems.FirstOrDefault(x => x.Id == _command.MenuItemId);
            typeof(MenuItem).GetTypeInfo().GetProperty("Status").SetValue(menuItem, MenuItemStatus.Deleted);
            Assert.Throws<Exception>(() => _menu.RemoveMenuItem(_command, _validatorMock.Object));
        }
    }
}
