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

namespace Weapsy.Domain.Tests.Menus
{
    [TestFixture]
    public class AddMenuItemTests
    {
        private Menu _menu;
        private AddMenuItem _command;
        private Mock<IValidator<AddMenuItem>> _validatorMock;
        private MenuItem _menuItem;
        private MenuItemLocalisation _firstMenuItemLocalisation;
        private MenuItemAdded _event;

        [SetUp]
        public void Setup()
        {
            _menu = new Menu();

            _command = new AddMenuItem
            {
                SiteId = _menu.SiteId,
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

            _validatorMock = new Mock<IValidator<AddMenuItem>>();
            _validatorMock.Setup(x => x.Validate(_command)).Returns(new ValidationResult());

            _menu.AddMenuItem(_command, _validatorMock.Object);

            _menuItem = _menu.MenuItems.FirstOrDefault(c => c.Id == _command.MenuItemId);

            _firstMenuItemLocalisation = _menuItem.MenuItemLocalisations.FirstOrDefault();

            _event = _menu.Events.OfType<MenuItemAdded>().SingleOrDefault();
        }

        [Test]
        public void Should_validate_command()
        {
            _validatorMock.Verify(x => x.Validate(_command));
        }

        [Test]
        public void Should_add_menu_item()
        {
            Assert.IsNotNull(_menuItem);
        }

        [Test]
        public void Should_set_sort_order()
        {
            Assert.AreEqual(1, _menuItem.SortOrder);
        }

        [Test]
        public void Should_set_menu_item_type()
        {
            Assert.AreEqual(_command.Type, _menuItem.Type);
        }

        [Test]
        public void Should_set_page_id()
        {
            Assert.AreEqual(_command.PageId, _menuItem.PageId);
        }

        [Test]
        public void Should_set_link()
        {
            Assert.AreEqual(_command.Link, _menuItem.Link);
        }

        [Test]
        public void Should_set_text()
        {
            Assert.AreEqual(_command.Text, _menuItem.Text);
        }

        [Test]
        public void Should_set_title()
        {
            Assert.AreEqual(_command.Title, _menuItem.Title);
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
        public void Should_throw_exception_if_menu_item_localisation_already_exists()
        {
            var languageId = Guid.NewGuid();
            _command.MenuItemLocalisations = new List<MenuItemLocalisation>
            {
                new MenuItemLocalisation
                {
                    LanguageId = languageId,
                    Text = "Text 1",
                    Title = "Title 1"
                },
                new MenuItemLocalisation
                {
                    LanguageId = languageId,
                    Text = "Text 2",
                    Title = "Title 2"
                }
            };
            Assert.Throws<Exception>(() => _menu.AddMenuItem(_command, _validatorMock.Object));
        }
    }
}
