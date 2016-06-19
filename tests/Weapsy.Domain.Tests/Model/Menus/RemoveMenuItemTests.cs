using System;
using System.Linq;
using NUnit.Framework;
using Moq;
using FluentValidation;
using FluentValidation.Results;
using Weapsy.Domain.Model.Menus;
using Weapsy.Domain.Model.Menus.Commands;
using System.Collections.Generic;
using Weapsy.Domain.Model.Menus.Events;
using System.Reflection;

namespace Weapsy.Domain.Tests.Menus
{
    [TestFixture]
    public class RemoveMenuItemTests
    {
        private Menu menu;
        private RemoveMenuItem command;
        private MenuItemRemoved @event;
        private Mock<IValidator<RemoveMenuItem>> validatorMock;

        [SetUp]
        public void Setup()
        {
            menu = new Menu();

            var addMenuItemCommand = new AddMenuItem
            {
                SiteId = Guid.NewGuid(),
                MenuId = Guid.NewGuid(),
                MenuItemId = Guid.NewGuid(),
                MenuItemType = MenuItemType.Link,
                PageId = Guid.NewGuid(),
                Link = "link",
                Text = "Text",
                Title = "Title",
                MenuItemLocalisations = new List<MenuItemDetails.MenuItemLocalisation>
                {
                    new MenuItemDetails.MenuItemLocalisation
                    {
                        LanguageId = Guid.NewGuid(),
                        Text = "Text 1",
                        Title = "Title 1"
                    },
                    new MenuItemDetails.MenuItemLocalisation
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

            command = new RemoveMenuItem
            {
                MenuId = menu.Id,
                MenuItemId = addMenuItemCommand.MenuItemId
            };

            validatorMock = new Mock<IValidator<RemoveMenuItem>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            menu.RemoveMenuItem(command, validatorMock.Object);

            @event = menu.Events.OfType<MenuItemRemoved>().SingleOrDefault();
        }

        [Test]
        public void Should_remove_menu_item()
        {
            Assert.AreEqual(MenuItemStatus.Deleted, menu.MenuItems.FirstOrDefault(x => x.Id == command.MenuItemId).Status);
        }

        [Test]
        public void Should_add_menu_item_removed_event()
        {
            Assert.IsNotNull(@event);
        }

        [Test]
        public void Should_set_id_in_menu_item_removed_event()
        {
            Assert.AreEqual(menu.Id, @event.AggregateRootId);
        }

        [Test]
        public void Should_set_site_id_in_menu_item_removed_event()
        {
            Assert.AreEqual(menu.SiteId, @event.SiteId);
        }

        [Test]
        public void Should_set_menu_item_in_menu_item_removed_event()
        {
            Assert.AreEqual(command.MenuItemId, @event.MenuItemId);
        }

        [Test]
        public void Should_throw_exception_if_menu_item_does_not_exist()
        {
            command = new RemoveMenuItem
            {
                MenuId = menu.Id,
                MenuItemId = Guid.NewGuid()
            };
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());
            Assert.Throws<Exception>(() => menu.RemoveMenuItem(command, validatorMock.Object));
        }

        [Test]
        public void Should_throw_exception_if_menu_item_is_deleted()
        {
            var menuItem = menu.MenuItems.FirstOrDefault(x => x.Id == command.MenuItemId);
            typeof(MenuItem).GetTypeInfo().GetProperty("MenuItemStatus").SetValue(menuItem, MenuItemStatus.Deleted);
            Assert.Throws<Exception>(() => menu.RemoveMenuItem(command, validatorMock.Object));
        }
    }
}
