using System;
using System.Linq;
using NUnit.Framework;
using Weapsy.Domain.Menus;
using System.Collections.Generic;
using Weapsy.Tests.Factories;
using Weapsy.Domain.Menus.Commands;
using Weapsy.Domain.Menus.Events;
using Weapsy.Framework.Identity;

namespace Weapsy.Domain.Tests.Menus
{
    [TestFixture]
    public class SetMenuItemPermissionsTests
    {
        private SetMenuItemPermissions _command;
        private Menu _menu;
        private MenuItem _menuItem;
        private MenuItemPermissionsSet _event;

        [SetUp]
        public void Setup()
        {
            var siteId = Guid.NewGuid();
            var menuId = Guid.NewGuid();
            var menuItemId = Guid.NewGuid();
            var menuName = "Name";

            _menu = MenuFactory.Menu(siteId, menuId, menuName, "My Item", "My Item Localised", menuItemId);
            _menuItem = _menu.MenuItems.FirstOrDefault(x => x.Id == menuItemId);

            _command = new SetMenuItemPermissions
            {
                SiteId = Guid.NewGuid(),
                MenuId = menuId,
                MenuItemId = menuItemId,
                MenuItemPermissions = new List<MenuItemPermission>
                {
                    new MenuItemPermission
                    {
                        MenuItemId = menuItemId,
                        RoleId = Everyone.Id
                    }
                }
            };

            _menu.SetMenuItemPermissions(_command);

            _event = _menu.Events.OfType<MenuItemPermissionsSet>().SingleOrDefault();
        }

        [Test]
        public void Should_set_module_permissions()
        {
            Assert.AreEqual(_command.MenuItemPermissions.FirstOrDefault().RoleId, _menuItem.MenuItemPermissions.FirstOrDefault().RoleId);
        }

        [Test]
        public void Should_add_menu_module_permissions_set_event()
        {
            Assert.IsNotNull(_event);
        }

        [Test]
        public void Should_set_site_id_in_menu_module_permissions_set_event()
        {
            Assert.AreEqual(_menu.SiteId, _event.SiteId);
        }

        [Test]
        public void Should_set_id_in_menu_module_permissions_set_event()
        {
            Assert.AreEqual(_menu.Id, _event.AggregateRootId);
        }

        [Test]
        public void Should_set_menu_permissions_in_menu_module_permissions_set_event()
        {
            Assert.AreEqual(_menuItem.MenuItemPermissions.FirstOrDefault().RoleId, _event.MenuItemPermissions.FirstOrDefault().RoleId);
        }
    }
}