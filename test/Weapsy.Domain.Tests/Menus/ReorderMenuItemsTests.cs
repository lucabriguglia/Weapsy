using System;
using NUnit.Framework;
using Moq;
using FluentValidation;
using FluentValidation.Results;
using Weapsy.Domain.Menus;
using Weapsy.Domain.Menus.Commands;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Weapsy.Domain.Tests.Menus
{
    [TestFixture]
    public class ReorderMenuItemsTests
    {
        private ReorderMenuItems _command;
        private Mock<IValidator<ReorderMenuItems>> _validatorMock;
        private Menu _menu;
        private MenuItem _menuItem1;
        private MenuItem _menuItem2;
        private MenuItem _menuItem3;

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

            _menu = Menu.CreateNew(createMenuCommand, createMenuValidatorMock.Object);

            var addMenuItem1 = new AddMenuItem
            {
                SiteId = _menu.SiteId,
                MenuId = _menu.Id,
                MenuItemId = Guid.NewGuid(),
                Type = MenuItemType.Page,
                PageId = Guid.NewGuid(),
                Text = "Menu Item 1"
            };

            var addMenuItem2 = new AddMenuItem
            {
                SiteId = _menu.SiteId,
                MenuId = _menu.Id,
                MenuItemId = Guid.NewGuid(),
                Type = MenuItemType.Page,
                PageId = Guid.NewGuid(),
                Text = "Menu Item 2"
            };

            var addMenuItem3 = new AddMenuItem
            {
                SiteId = _menu.SiteId,
                MenuId = _menu.Id,
                MenuItemId = Guid.NewGuid(),
                Type = MenuItemType.Page,
                PageId = Guid.NewGuid(),
                Text = "Menu Item 3"
            };

            var addMenuItemValidatorMock = new Mock<IValidator<AddMenuItem>>();
            addMenuItemValidatorMock.Setup(x => x.Validate(addMenuItem1)).Returns(new ValidationResult());
            addMenuItemValidatorMock.Setup(x => x.Validate(addMenuItem2)).Returns(new ValidationResult());
            addMenuItemValidatorMock.Setup(x => x.Validate(addMenuItem3)).Returns(new ValidationResult());

            _menu.AddMenuItem(addMenuItem1, addMenuItemValidatorMock.Object);
            _menu.AddMenuItem(addMenuItem2, addMenuItemValidatorMock.Object);
            _menu.AddMenuItem(addMenuItem3, addMenuItemValidatorMock.Object);

            _command = new ReorderMenuItems
            {
                Id = _menu.Id,
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
            validatorMock.Setup(x => x.Validate(_command)).Returns(new ValidationResult());

            _menu.ReorderMenuItems(_command, validatorMock.Object);

            _menuItem1 = _menu.MenuItems.FirstOrDefault(x => x.Id == addMenuItem1.MenuItemId);
            _menuItem2 = _menu.MenuItems.FirstOrDefault(x => x.Id == addMenuItem2.MenuItemId);
            _menuItem3 = _menu.MenuItems.FirstOrDefault(x => x.Id == addMenuItem3.MenuItemId);
        }

        [Test]
        public void Should_move_menu_item_2_one_position_down()
        {
            Assert.AreEqual(2, _menuItem2.SortOrder);
        }

        [Test]
        public void Should_move_menu_item_3_under_menu_item_1()
        {
            Assert.AreEqual(_menuItem1.Id, _menuItem3.ParentId);
        }

        [Test]
        public void Should_move_menu_item_3_at_first_position()
        {
            Assert.AreEqual(1, _menuItem3.SortOrder);
        }

        [Test]
        public void Should_throw_exception_if_parent_menu_item_does_not_exist()
        {
            _command = new ReorderMenuItems
            {
                Id = _menu.Id,
                MenuItems = new List<ReorderMenuItems.MenuItem>
                {
                    new ReorderMenuItems.MenuItem
                    {
                        ParentId = Guid.NewGuid()
                    }
                }
            };
            var validatorMock = new Mock<IValidator<ReorderMenuItems>>();
            validatorMock.Setup(x => x.Validate(_command)).Returns(new ValidationResult());
            Assert.Throws<Exception>(() => _menu.ReorderMenuItems(_command, validatorMock.Object));
        }

        [Test]
        public void Should_throw_exception_if_menu_item_does_not_exist()
        {
            _command = new ReorderMenuItems
            {
                Id = _menu.Id,
                MenuItems = new List<ReorderMenuItems.MenuItem>
                {
                    new ReorderMenuItems.MenuItem
                    {
                        Id = Guid.NewGuid()
                    }
                }
            };
            var validatorMock = new Mock<IValidator<ReorderMenuItems>>();
            validatorMock.Setup(x => x.Validate(_command)).Returns(new ValidationResult());
            Assert.Throws<Exception>(() => _menu.ReorderMenuItems(_command, validatorMock.Object));
        }

        [Test]
        public void Should_throw_exception_if_menu_item_is_deleted()
        {
            var menuItem = _menu.MenuItems.FirstOrDefault(x => x.Id == _command.MenuItems.FirstOrDefault().Id);
            typeof(MenuItem).GetTypeInfo().GetProperty("Status").SetValue(menuItem, MenuItemStatus.Deleted);
            var validatorMock = new Mock<IValidator<ReorderMenuItems>>();
            validatorMock.Setup(x => x.Validate(_command)).Returns(new ValidationResult());
            Assert.Throws<Exception>(() => _menu.ReorderMenuItems(_command, validatorMock.Object));
        }
    }
}