using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Weapsy.Domain.Data.SqlServer.Repositories;
using Weapsy.Domain.Model.Menus;
using Weapsy.Tests.Factories;
using MenuDbEntity = Weapsy.Domain.Data.SqlServer.Entities.Menu;
using MenuItemDbEntity = Weapsy.Domain.Data.SqlServer.Entities.MenuItem;
using MenuItemLocalisationDbEntity = Weapsy.Domain.Data.SqlServer.Entities.MenuItemLocalisation;

namespace Weapsy.Domain.Data.SqlServer.Tests
{
    [TestFixture]
    public class MenuRepositoryTests
    {
        private IMenuRepository sut;
        private Guid siteId;
        private Guid menuId1;
        private Guid menuId2;
        private Guid menuItemId1;
        private Guid menuItemId2;
        private Guid language1;

        [SetUp]
        public void SetUp()
        {
            var optionsBuilder = new DbContextOptionsBuilder<WeapsyDbContext>();
            optionsBuilder.UseInMemoryDatabase();
            var dbContext = new WeapsyDbContext(optionsBuilder.Options);

            siteId = Guid.NewGuid();
            menuId1 = Guid.NewGuid();
            menuId2 = Guid.NewGuid();
            menuItemId1 = Guid.NewGuid();
            menuItemId2 = Guid.NewGuid();
            language1 = Guid.NewGuid();

            dbContext.Set<MenuDbEntity>().AddRange(
                new MenuDbEntity
                {
                    SiteId = siteId,
                    Id = menuId1,
                    Name = "Menu 1",
                    Status = MenuStatus.Active,
                    MenuItems = new List<MenuItemDbEntity>
                    {
                        new MenuItemDbEntity
                        {
                            MenuId = menuId1,
                            Id = menuItemId1,
                            Text = "Menu Item 1",
                            Status = MenuItemStatus.Active,
                            MenuItemLocalisations = new List<MenuItemLocalisationDbEntity>
                            {
                                new MenuItemLocalisationDbEntity
                                {
                                    MenuItemId = menuItemId1,
                                    LanguageId = language1,
                                    Text = "Menu Item 1 Localisation 1"
                                },
                                new MenuItemLocalisationDbEntity
                                {
                                    MenuItemId = menuItemId1,
                                    LanguageId = Guid.NewGuid(),
                                    Text = "Menu Item 1 Localisation 2"
                                }
                            }
                        },
                        new MenuItemDbEntity
                        {
                            MenuId = menuId1,
                            Id = menuItemId2,
                            Text = "Menu Item 2",
                            Status = MenuItemStatus.Deleted
                        }
                    }
                },
                new MenuDbEntity
                {
                    SiteId = siteId,
                    Id = menuId2,
                    Name = "Menu 2",
                    Status = MenuStatus.Active
                },
                new MenuDbEntity
                {
                    Status = MenuStatus.Deleted
                }
            );

            dbContext.SaveChanges();

            var mapperMock = new Moq.Mock<AutoMapper.IMapper>();
            mapperMock.Setup(x => x.Map<MenuDbEntity>(Moq.It.IsAny<Menu>())).Returns(new MenuDbEntity());
            mapperMock.Setup(x => x.Map<Menu>(Moq.It.IsAny<MenuDbEntity>())).Returns(new Menu());

            sut = new MenuRepository(dbContext, mapperMock.Object);
        }

        [Test]
        public void Should_return_menu_by_id_only()
        {
            var actual = sut.GetById(menuId1);
            Assert.NotNull(actual);
        }

        [Test]
        public void Should_return_menu_by_id_only_with_no_deleted_menu_items()
        {
            var actual = sut.GetById(menuId1);
            Assert.NotNull(actual);
        }

        [Test]
        public void Should_return_menu_by_id()
        {
            var actual = sut.GetById(siteId, menuId1);
            Assert.NotNull(actual);
        }

        [Test]
        public void Should_return_menu_by_id_with_no_deleted_menu_items()
        {
            var actual = sut.GetById(siteId, menuId1);
            Assert.AreEqual(0, actual.MenuItems.Where(x => x.Status == MenuItemStatus.Deleted).Count());
        }

        [Test]
        public void Should_return_menu_by_name()
        {
            var actual = sut.GetByName(siteId, "Menu 1");
            Assert.NotNull(actual);
        }

        [Test]
        public void Should_return_menu_by_name_with_no_deleted_menu_items()
        {
            var actual = sut.GetByName(siteId, "Menu 1");
            Assert.AreEqual(0, actual.MenuItems.Where(x => x.Status == MenuItemStatus.Deleted).Count());
        }

        [Test]
        public void Should_return_all_menus()
        {
            var actual = sut.GetAll(siteId);
            Assert.AreEqual(2, actual.Count);
        }

        [Test]
        public void Should_return_all_menus_with_no_deleted_menu_items()
        {
            var actual = sut.GetAll(siteId);
            foreach (var item in actual)
                Assert.AreEqual(0, item.MenuItems.Where(x => x.Status == MenuItemStatus.Deleted).Count());        
        }

        [Test]
        public void Should_save_new_menu()
        {
            var newMenu = MenuFactory.Menu(siteId, Guid.NewGuid(), "Menu 3", "Item", "");

            sut.Create(newMenu);

            var actual = sut.GetById(siteId, newMenu.Id);

            Assert.NotNull(actual);
        }

        [Test]
        public void Should_update_menu()
        {
            var newMenuName = "New Menu 1";
            var newMenuItemText = "New Menu Item 1";
            var newMenuItemLocalisationText = "New Menu Item 1 Localisation 1";

            var menuToUpdate = MenuFactory.Menu(siteId, Guid.NewGuid(), newMenuName, newMenuItemText, newMenuItemLocalisationText);

            sut.Update(menuToUpdate);

            var updatedMenu = sut.GetById(siteId, menuId1);
            var updatedMenuItem = updatedMenu.MenuItems.FirstOrDefault();
            var updatedMenuItemLocalisation = updatedMenuItem.MenuItemLocalisations.FirstOrDefault();

            Assert.AreEqual(newMenuName, updatedMenu.Name);
            Assert.AreEqual(newMenuItemText, updatedMenuItem.Text);
            Assert.AreEqual(newMenuItemLocalisationText, updatedMenuItemLocalisation.Text);
        }
    }
}
