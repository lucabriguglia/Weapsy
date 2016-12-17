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
    public class UpdateMenuItemTests
    {
        private Menu _menu;
        private UpdateMenuItem _command;
        private Mock<IValidator<UpdateMenuItem>> _validatorMock;
        private MenuItem _menuItem;
        private MenuItemLocalisation _firstMenuItemLocalisation;
        private MenuItemUpdated _event;

        [SetUp]
        public void Setup()
        {
            _menu = new Menu();

            var menuItemId = Guid.NewGuid();

            var addMenuItemCommand = new AddMenuItem
            {
                SiteId = _menu.SiteId,
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

            _menu.AddMenuItem(addMenuItemCommand, addMenuItemValidatorMock.Object);

            _command = new UpdateMenuItem
            {
                SiteId = _menu.SiteId,
                MenuId = _menu.Id,
                MenuItemId = menuItemId,
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

            _validatorMock = new Mock<IValidator<UpdateMenuItem>>();
            _validatorMock.Setup(x => x.Validate(_command)).Returns(new ValidationResult());

            _menu.UpdateMenuItem(_command, _validatorMock.Object);

            _menuItem = _menu.MenuItems.FirstOrDefault(c => c.Id == _command.MenuItemId);

            _firstMenuItemLocalisation = _menuItem.MenuItemLocalisations.FirstOrDefault();

            _event = _menu.Events.OfType<MenuItemUpdated>().SingleOrDefault();
        }

        [Test]
        public void Should_validate_command()
        {
            _validatorMock.Verify(x => x.Validate(_command));
        }

        [Test]
        public void Should_set_menu_item_localisations()
        {
            Assert.AreEqual(_command.MenuItemLocalisations.Count(), _menuItem.MenuItemLocalisations.Count());
        }

        [Test]
        public void Should_set_localisation_language_id()
        {
            Assert.AreEqual(_command.MenuItemLocalisations[0].LanguageId, _firstMenuItemLocalisation.LanguageId);
        }

        [Test]
        public void Should_set_localisation_menu_item_id()
        {
            Assert.AreEqual(_command.MenuItemId, _firstMenuItemLocalisation.MenuItemId);
        }

        [Test]
        public void Should_set_localisation_text()
        {
            Assert.AreEqual(_command.MenuItemLocalisations[0].Text, _firstMenuItemLocalisation.Text);
        }

        [Test]
        public void Should_set_localisation_title()
        {
            Assert.AreEqual(_command.MenuItemLocalisations[0].Title, _firstMenuItemLocalisation.Title);
        }

        [Test]
        public void Should_add_menu_item_added_event()
        {
            Assert.IsNotNull(_event);
        }

        [Test]
        public void Should_set_id_in_menu_item_added_event()
        {
            Assert.AreEqual(_menu.Id, _event.AggregateRootId);
        }

        [Test]
        public void Should_set_site_id_in_menu_item_added_event()
        {
            Assert.AreEqual(_command.SiteId, _event.SiteId);
        }

        [Test]
        public void Should_set_menu_item_id_in_menu_item_added_event()
        {
            Assert.AreEqual(_menuItem.Id, _event.MenuItem.Id);
        }

        [Test]
        public void Should_set_link_in_menu_item_added_event()
        {
            Assert.AreEqual(_menuItem.Type, _event.MenuItem.Type);
        }

        [Test]
        public void Should_set_page_id_in_menu_item_added_event()
        {
            Assert.AreEqual(_menuItem.PageId, _event.MenuItem.PageId);
        }

        [Test]
        public void Should_set_parent_id_in_menu_item_added_event()
        {
            Assert.AreEqual(_menuItem.ParentId, _event.MenuItem.ParentId);
        }

        [Test]
        public void Should_set_sort_order_in_menu_item_added_event()
        {
            Assert.AreEqual(_menuItem.SortOrder, _event.MenuItem.SortOrder);
        }

        [Test]
        public void Should_set_localisations_in_menu_item_added_event()
        {
            Assert.AreEqual(_menuItem.MenuItemLocalisations, _event.MenuItem.MenuItemLocalisations);
        }

        [Test]
        public void Should_throw_exception_if_menu_item_does_not_exist()
        {
            _command = new UpdateMenuItem
            {
                MenuItemId = Guid.NewGuid(),
            };
            Assert.Throws<Exception>(() => _menu.UpdateMenuItem(_command, _validatorMock.Object));
        }

        [Test]
        public void Should_throw_exception_if_menu_item_is_deleted()
        {
            var menuItem = _menu.MenuItems.FirstOrDefault(x => x.Id == _command.MenuItemId);
            typeof(MenuItem).GetTypeInfo().GetProperty("Status").SetValue(menuItem, MenuItemStatus.Deleted);
            Assert.Throws<Exception>(() => _menu.UpdateMenuItem(_command, _validatorMock.Object));
        }
    }
}
