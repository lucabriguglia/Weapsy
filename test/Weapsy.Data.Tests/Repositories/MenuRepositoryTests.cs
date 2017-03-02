using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Weapsy.Domain.Menus;
using Weapsy.Tests.Factories;
using MenuDbEntity = Weapsy.Data.Entities.Menu;
using MenuItemDbEntity = Weapsy.Data.Entities.MenuItem;
using MenuItemLocalisationDbEntity = Weapsy.Data.Entities.MenuItemLocalisation;
using System.Linq;
using Weapsy.Data.Domain;

namespace Weapsy.Data.Tests.Repositories
{
    [TestFixture]
    public class MenuRepositoryTests
    {
        private DbContextOptions<WeapsyDbContext> _contextOptions;
        private Guid _siteId;
        private Guid _menuId1;
        private Guid _menuId2;
        private Guid _menuItemId1;
        private Guid _menuItemId2;
        private Guid _language1;
        private Guid _deletedMenuId;

        [SetUp]
        public void SetUp()
        {
            _contextOptions = DbContextShared.CreateContextOptions();

            using (var context = new WeapsyDbContext(_contextOptions))
            {
                _siteId = Guid.NewGuid();
                _menuId1 = Guid.NewGuid();
                _menuId2 = Guid.NewGuid();
                _menuItemId1 = Guid.NewGuid();
                _menuItemId2 = Guid.NewGuid();
                _language1 = Guid.NewGuid();
                _deletedMenuId = Guid.NewGuid();

                context.Menus.AddRange(
                    new MenuDbEntity
                    {
                        SiteId = _siteId,
                        Id = _menuId1,
                        Name = "Menu 1",
                        Status = MenuStatus.Active,
                        MenuItems = new List<MenuItemDbEntity>
                        {
                            new MenuItemDbEntity
                            {
                                MenuId = _menuId1,
                                Id = _menuItemId1,
                                Text = "Menu Item 1",
                                Status = MenuItemStatus.Active,
                                MenuItemLocalisations = new List<MenuItemLocalisationDbEntity>
                                {
                                    new MenuItemLocalisationDbEntity
                                    {
                                        MenuItemId = _menuItemId1,
                                        LanguageId = _language1,
                                        Text = "Menu Item 1 Localisation 1"
                                    },
                                    new MenuItemLocalisationDbEntity
                                    {
                                        MenuItemId = _menuItemId1,
                                        LanguageId = Guid.NewGuid(),
                                        Text = "Menu Item 1 Localisation 2"
                                    }
                                }
                            },
                            new MenuItemDbEntity
                            {
                                MenuId = _menuId1,
                                Id = _menuItemId2,
                                Text = "Menu Item 2",
                                Status = MenuItemStatus.Deleted
                            }
                        }
                    },
                    new MenuDbEntity
                    {
                        SiteId = _siteId,
                        Id = _menuId2,
                        Name = "Menu 2",
                        Status = MenuStatus.Active
                    },
                    new MenuDbEntity
                    {
                        Id = _deletedMenuId,
                        Status = MenuStatus.Deleted
                    }
                );

                context.SaveChanges();
            }
        }

        [Test]
        public void Should_return_null_if_menu_is_deleted()
        {
            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new MenuRepository(DbContextShared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                var menu = repository.GetById(_deletedMenuId);

                Assert.Null(menu);
            }
        }

        [Test]
        public void Should_return_menu_by_id_only()
        {
            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new MenuRepository(DbContextShared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                var menu = repository.GetById(_menuId1);

                Assert.NotNull(menu);
            }
        }

        [Test]
        public void Should_return_menu_by_id_only_with_no_deleted_menu_items()
        {
            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new MenuRepository(DbContextShared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                var menu = repository.GetById(_menuId1);

                Assert.AreEqual(0, menu.MenuItems.Where(x => x.Status == MenuItemStatus.Deleted).Count());
            }
        }

        [Test]
        public void Should_return_menu_by_id()
        {
            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new MenuRepository(DbContextShared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                var menu = repository.GetById(_siteId, _menuId1);

                Assert.NotNull(menu);
            }
        }

        [Test]
        public void Should_return_menu_by_id_with_no_deleted_menu_items()
        {
            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new MenuRepository(DbContextShared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                var menu = repository.GetById(_siteId, _menuId1);

                Assert.AreEqual(0, menu.MenuItems.Where(x => x.Status == MenuItemStatus.Deleted).Count());
            }
        }

        [Test]
        public void Should_return_menu_by_name()
        {
            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new MenuRepository(DbContextShared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                var menu = repository.GetByName(_siteId, "Menu 1");

                Assert.NotNull(menu);
            }
        }

        [Test]
        public void Should_return_menu_by_name_with_no_deleted_menu_items()
        {
            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new MenuRepository(DbContextShared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                var menu = repository.GetByName(_siteId, "Menu 1");

                Assert.AreEqual(0, menu.MenuItems.Where(x => x.Status == MenuItemStatus.Deleted).Count());
            }
        }

        [Test]
        public void Should_save_new_menu()
        {
            var newMenu = MenuFactory.Menu(_siteId, Guid.NewGuid(), "Menu 3", "Item", "");

            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new MenuRepository(DbContextShared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                repository.Create(newMenu);
            }

            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new MenuRepository(DbContextShared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                var menu = repository.GetById(_siteId, newMenu.Id);

                Assert.NotNull(menu);
            }
        }

        [Test]
        public void Should_update_menu()
        {
            const string newMenuName = "New Menu 1";
            const string newMenuItemText = "New Menu Item 1";
            const string newMenuItemLocalisationText = "New Menu Item 1 Localisation 1";

            var menuToUpdate = MenuFactory.Menu(_siteId, _menuId1, newMenuName, newMenuItemText, newMenuItemLocalisationText, _menuItemId1, _language1);

            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new MenuRepository(DbContextShared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                repository.Update(menuToUpdate);
            }

            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new MenuRepository(DbContextShared.CreateNewContextFactory(context), Shared.CreateNewMapper());

                var updatedMenu = repository.GetById(_siteId, _menuId1);
                var updatedMenuItem = updatedMenu.MenuItems.FirstOrDefault();
                var updatedMenuItemLocalisation = updatedMenuItem.MenuItemLocalisations.FirstOrDefault();

                Assert.AreEqual(newMenuName, updatedMenu.Name);
                Assert.AreEqual(newMenuItemText, updatedMenuItem.Text);
                Assert.AreEqual(newMenuItemLocalisationText, updatedMenuItemLocalisation.Text);
            }
        }
    }
}
