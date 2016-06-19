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

namespace Weapsy.Domain.Tests.Menus
{
    [TestFixture]
    public class AddMenuItemTests
    {
        private Menu menu;
        private AddMenuItem command;
        private Mock<IValidator<AddMenuItem>> validatorMock;
        private MenuItem menuItem;
        private MenuItemLocalisation firstMenuItemLocalisation;
        private MenuItemAdded @event;

        [SetUp]
        public void Setup()
        {
            menu = new Menu();

            command = new AddMenuItem
            {
                SiteId = menu.SiteId,
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

            validatorMock = new Mock<IValidator<AddMenuItem>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            menu.AddMenuItem(command, validatorMock.Object);

            menuItem = menu.MenuItems.FirstOrDefault(c => c.Id == command.MenuItemId);

            firstMenuItemLocalisation = menuItem.MenuItemLocalisations.FirstOrDefault();

            @event = menu.Events.OfType<MenuItemAdded>().SingleOrDefault();
        }

        [Test]
        public void Should_validate_command()
        {
            validatorMock.Verify(x => x.Validate(command));
        }

        [Test]
        public void Should_add_menu_item()
        {
            Assert.IsNotNull(menuItem);
        }

        [Test]
        public void Should_set_sort_order()
        {
            Assert.AreEqual(1, menuItem.SortOrder);
        }

        [Test]
        public void Should_set_menu_item_type()
        {
            Assert.AreEqual(command.MenuItemType, menuItem.MenuItemType);
        }

        [Test]
        public void Should_set_page_id()
        {
            Assert.AreEqual(command.PageId, menuItem.PageId);
        }

        [Test]
        public void Should_set_link()
        {
            Assert.AreEqual(command.Link, menuItem.Link);
        }

        [Test]
        public void Should_set_text()
        {
            Assert.AreEqual(command.Text, menuItem.Text);
        }

        [Test]
        public void Should_set_title()
        {
            Assert.AreEqual(command.Title, menuItem.Title);
        }

        [Test]
        public void Should_set_menu_item_localisations()
        {
            Assert.AreEqual(command.MenuItemLocalisations.Count(), menuItem.MenuItemLocalisations.Count());
        }

        [Test]
        public void Should_set_localisation_language_id()
        {
            Assert.AreEqual(command.MenuItemLocalisations[0].LanguageId, firstMenuItemLocalisation.LanguageId);
        }

        [Test]
        public void Should_set_localisation_menu_item_id()
        {
            Assert.AreEqual(command.MenuItemId, firstMenuItemLocalisation.MenuItemId);
        }

        [Test]
        public void Should_set_localisation_text()
        {
            Assert.AreEqual(command.MenuItemLocalisations[0].Text, firstMenuItemLocalisation.Text);
        }

        [Test]
        public void Should_set_localisation_title()
        {
            Assert.AreEqual(command.MenuItemLocalisations[0].Title, firstMenuItemLocalisation.Title);
        }

        [Test]
        public void Should_add_menu_item_added_event()
        {
            Assert.IsNotNull(@event);
        }

        [Test]
        public void Should_set_id_in_menu_item_added_event()
        {
            Assert.AreEqual(menu.Id, @event.AggregateRootId);
        }

        [Test]
        public void Should_set_site_id_in_menu_item_added_event()
        {
            Assert.AreEqual(command.SiteId, @event.SiteId);
        }

        [Test]
        public void Should_set_menu_item_id_in_menu_item_added_event()
        {
            Assert.AreEqual(menuItem.Id, @event.MenuItem.Id);
        }

        [Test]
        public void Should_set_link_in_menu_item_added_event()
        {
            Assert.AreEqual(menuItem.MenuItemType, @event.MenuItem.MenuItemType);
        }

        [Test]
        public void Should_set_page_id_in_menu_item_added_event()
        {
            Assert.AreEqual(menuItem.PageId, @event.MenuItem.PageId);
        }

        [Test]
        public void Should_set_parent_id_in_menu_item_added_event()
        {
            Assert.AreEqual(menuItem.ParentId, @event.MenuItem.ParentId);
        }

        [Test]
        public void Should_set_sort_order_in_menu_item_added_event()
        {
            Assert.AreEqual(menuItem.SortOrder, @event.MenuItem.SortOrder);
        }

        [Test]
        public void Should_set_localisations_in_menu_item_added_event()
        {
            Assert.AreEqual(menuItem.MenuItemLocalisations, @event.MenuItem.MenuItemLocalisations);
        }

        [Test]
        public void Should_throw_exception_if_menu_item_localisation_already_exists()
        {
            var languageId = Guid.NewGuid();
            command.MenuItemLocalisations = new List<MenuItemDetails.MenuItemLocalisation>
            {
                new MenuItemDetails.MenuItemLocalisation
                {
                    LanguageId = languageId,
                    Text = "Text 1",
                    Title = "Title 1"
                },
                new MenuItemDetails.MenuItemLocalisation
                {
                    LanguageId = languageId,
                    Text = "Text 2",
                    Title = "Title 2"
                }
            };
            Assert.Throws<Exception>(() => menu.AddMenuItem(command, validatorMock.Object));
        }
    }
}
