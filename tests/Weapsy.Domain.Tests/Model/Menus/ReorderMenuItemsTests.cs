using System;
using NUnit.Framework;
using Moq;
using FluentValidation;
using FluentValidation.Results;
using Weapsy.Domain.Model.Menus;
using Weapsy.Domain.Model.Menus.Commands;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Weapsy.Domain.Tests.Menus
{
    [TestFixture]
    public class ReorderMenuItemsTests
    {
        private ReorderMenuItems command;
        private Mock<IValidator<ReorderMenuItems>> validatorMock;
        private Menu menu;
        private MenuItem menuItem1;
        private MenuItem menuItem2;
        private MenuItem menuItem3;

        [SetUp]
        public void Setup()
        {
            var createMenuCommand = new CreateMenu
            {
                SiteId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Name = "My Menu"
            };

            var createMenuValidatorMock = new Mock<IValidator<CreateMenu>>();
            createMenuValidatorMock.Setup(x => x.Validate(createMenuCommand)).Returns(new ValidationResult());

            menu = Menu.CreateNew(createMenuCommand, createMenuValidatorMock.Object);

            var addMenuItem1 = new AddMenuItem
            {
                SiteId = menu.SiteId,
                MenuId = menu.Id,
                MenuItemId = Guid.NewGuid(),
                MenuItemType = MenuItemType.Page,
                PageId = Guid.NewGuid(),
                Text = "Menu Item 1"
            };

            var addMenuItem2 = new AddMenuItem
            {
                SiteId = menu.SiteId,
                MenuId = menu.Id,
                MenuItemId = Guid.NewGuid(),
                MenuItemType = MenuItemType.Page,
                PageId = Guid.NewGuid(),
                Text = "Menu Item 2"
            };

            var addMenuItem3 = new AddMenuItem
            {
                SiteId = menu.SiteId,
                MenuId = menu.Id,
                MenuItemId = Guid.NewGuid(),
                MenuItemType = MenuItemType.Page,
                PageId = Guid.NewGuid(),
                Text = "Menu Item 3"
            };

            var addMenuItemValidatorMock = new Mock<IValidator<AddMenuItem>>();
            addMenuItemValidatorMock.Setup(x => x.Validate(addMenuItem1)).Returns(new ValidationResult());
            addMenuItemValidatorMock.Setup(x => x.Validate(addMenuItem2)).Returns(new ValidationResult());
            addMenuItemValidatorMock.Setup(x => x.Validate(addMenuItem3)).Returns(new ValidationResult());

            menu.AddMenuItem(addMenuItem1, addMenuItemValidatorMock.Object);
            menu.AddMenuItem(addMenuItem2, addMenuItemValidatorMock.Object);
            menu.AddMenuItem(addMenuItem3, addMenuItemValidatorMock.Object);

            command = new ReorderMenuItems
            {
                Id = menu.Id,
                MenuItems = new List<ReorderMenuItems.MenuItem>
                {
                    new ReorderMenuItems.MenuItem
                    {
                        Id = addMenuItem1.MenuItemId
                    },
                    new ReorderMenuItems.MenuItem
                    {
                        Id = addMenuItem3.MenuItemId,
                        ParentId = addMenuItem1.MenuItemId
                    },
                    new ReorderMenuItems.MenuItem
                    {
                        Id = addMenuItem2.MenuItemId,
                        ParentId = addMenuItem1.MenuItemId
                    }
                }
            };

            var validatorMock = new Mock<IValidator<ReorderMenuItems>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            menu.ReorderMenuItems(command, validatorMock.Object);

            menuItem1 = menu.MenuItems.FirstOrDefault(x => x.Id == addMenuItem1.MenuItemId);
            menuItem2 = menu.MenuItems.FirstOrDefault(x => x.Id == addMenuItem2.MenuItemId);
            menuItem3 = menu.MenuItems.FirstOrDefault(x => x.Id == addMenuItem3.MenuItemId);
        }

        [Test]
        public void Should_move_menu_item_2_one_position_down()
        {
            Assert.AreEqual(2, menuItem2.SortOrder);
        }

        [Test]
        public void Should_move_menu_item_3_under_menu_item_1()
        {
            Assert.AreEqual(menuItem1.Id, menuItem3.ParentId);
        }

        [Test]
        public void Should_move_menu_item_3_at_first_position()
        {
            Assert.AreEqual(1, menuItem3.SortOrder);
        }

        [Test]
        public void Should_throw_exception_if_parent_menu_item_does_not_exist()
        {
            command = new ReorderMenuItems
            {
                Id = menu.Id,
                MenuItems = new List<ReorderMenuItems.MenuItem>
                {
                    new ReorderMenuItems.MenuItem
                    {
                        ParentId = Guid.NewGuid()
                    }
                }
            };
            var validatorMock = new Mock<IValidator<ReorderMenuItems>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());
            Assert.Throws<Exception>(() => menu.ReorderMenuItems(command, validatorMock.Object));
        }

        [Test]
        public void Should_throw_exception_if_menu_item_does_not_exist()
        {
            command = new ReorderMenuItems
            {
                Id = menu.Id,
                MenuItems = new List<ReorderMenuItems.MenuItem>
                {
                    new ReorderMenuItems.MenuItem
                    {
                        Id = Guid.NewGuid()
                    }
                }
            };
            var validatorMock = new Mock<IValidator<ReorderMenuItems>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());
            Assert.Throws<Exception>(() => menu.ReorderMenuItems(command, validatorMock.Object));
        }

        [Test]
        public void Should_throw_exception_if_menu_item_is_deleted()
        {
            var menuItem = menu.MenuItems.FirstOrDefault(x => x.Id == command.MenuItems.FirstOrDefault().Id);
            typeof(MenuItem).GetTypeInfo().GetProperty("MenuItemStatus").SetValue(menuItem, MenuItemStatus.Deleted);
            var validatorMock = new Mock<IValidator<ReorderMenuItems>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());
            Assert.Throws<Exception>(() => menu.ReorderMenuItems(command, validatorMock.Object));
        }
    }
}