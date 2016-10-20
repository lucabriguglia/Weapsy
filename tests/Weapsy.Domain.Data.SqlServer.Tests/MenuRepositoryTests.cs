using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Weapsy.Domain.Data.SqlServer.Repositories;
using Weapsy.Domain.Menus;
using Weapsy.Tests.Factories;
using MenuDbEntity = Weapsy.Domain.Data.SqlServer.Entities.Menu;
using MenuItemDbEntity = Weapsy.Domain.Data.SqlServer.Entities.MenuItem;
using MenuItemLocalisationDbEntity = Weapsy.Domain.Data.SqlServer.Entities.MenuItemLocalisation;

namespace Weapsy.Domain.Data.SqlServer.Tests
{
    [TestFixture]
    public class MenuRepositoryTests
    {
        private IMenuRepository _sut;
        private WeapsyDbContext _dbContext;
        private Guid _siteId;
        private Guid _menuId1;
        private Guid _menuId2;
        private Guid _menuItemId1;
        private Guid _menuItemId2;
        private Guid _language1;

        [SetUp]
        public void SetUp()
        {
            var optionsBuilder = new DbContextOptionsBuilder<WeapsyDbContext>();
            optionsBuilder.UseInMemoryDatabase();
            _dbContext = new WeapsyDbContext(optionsBuilder.Options);

            _siteId = Guid.NewGuid();
            _menuId1 = Guid.NewGuid();
            _menuId2 = Guid.NewGuid();
            _menuItemId1 = Guid.NewGuid();
            _menuItemId2 = Guid.NewGuid();
            _language1 = Guid.NewGuid();

            _dbContext.Set<MenuDbEntity>().AddRange(
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
                    Status = MenuStatus.Deleted
                }
            );

            _dbContext.SaveChanges();

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(x => x.Map<MenuDbEntity>(It.IsAny<Menu>())).Returns(new MenuDbEntity());
            mapperMock.Setup(x => x.Map<Menu>(It.IsAny<MenuDbEntity>())).Returns(new Menu());
            mapperMock.Setup(x => x.Map<ICollection<Menu>>(It.IsAny<ICollection<MenuDbEntity>>())).Returns(new List<Menu>
            {
                MenuFactory.Menu(_siteId, _menuId1, "Name", "ItemText", "ItemTextLocalised"),
                MenuFactory.Menu(_siteId, _menuId2, "Name", "ItemText", "ItemTextLocalised")
            });

            _sut = new MenuRepository(_dbContext, mapperMock.Object);
        }

        [Test]
        public void Should_return_menu_by_id_only()
        {
            var actual = _sut.GetById(_menuId1);
            Assert.NotNull(actual);
        }

        [Test]
        public void Should_return_menu_by_id_only_with_no_deleted_menu_items()
        {
            var actual = _sut.GetById(_menuId1);
            Assert.NotNull(actual);
        }

        [Test]
        public void Should_return_menu_by_id()
        {
            var actual = _sut.GetById(_siteId, _menuId1);
            Assert.NotNull(actual);
        }

        [Test]
        public void Should_return_menu_by_id_with_no_deleted_menu_items()
        {
            var actual = _sut.GetById(_siteId, _menuId1);
            Assert.AreEqual(0, actual.MenuItems.Where(x => x.Status == MenuItemStatus.Deleted).Count());
        }

        [Test]
        public void Should_return_menu_by_name()
        {
            var actual = _sut.GetByName(_siteId, "Menu 1");
            Assert.NotNull(actual);
        }

        [Test]
        public void Should_return_menu_by_name_with_no_deleted_menu_items()
        {
            var actual = _sut.GetByName(_siteId, "Menu 1");
            Assert.AreEqual(0, actual.MenuItems.Where(x => x.Status == MenuItemStatus.Deleted).Count());
        }

        [Test]
        public void Should_return_all_menus()
        {
            var actual = _sut.GetAll(_siteId);
            Assert.AreEqual(2, actual.Count);
        }

        [Test]
        public void Should_return_all_menus_with_no_deleted_menu_items()
        {
            var actual = _sut.GetAll(_siteId);
            foreach (var item in actual)
                Assert.AreEqual(0, item.MenuItems.Where(x => x.Status == MenuItemStatus.Deleted).Count());        
        }

        [Test]
        public void Should_save_new_menu()
        {
            var newMenu = MenuFactory.Menu(_siteId, Guid.NewGuid(), "Menu 3", "Item", "");
            var newMenuDbEntity = new MenuDbEntity
            {
                SiteId = newMenu.SiteId,
                Id = newMenu.Id,
                Name = newMenu.Name
            };

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(x => x.Map<MenuDbEntity>(newMenu)).Returns(newMenuDbEntity);
            mapperMock.Setup(x => x.Map<Menu>(newMenuDbEntity)).Returns(newMenu);

            _sut = new MenuRepository(_dbContext, mapperMock.Object);

            _sut.Create(newMenu);

            var actual = _sut.GetById(_siteId, newMenu.Id);

            Assert.NotNull(actual);
        }

        [Test]
        public void Should_update_menu()
        {
            var newMenuName = "New Menu 1";
            var newMenuItemText = "New Menu Item 1";
            var newMenuItemLocalisationText = "New Menu Item 1 Localisation 1";

            var menuToUpdate = MenuFactory.Menu(_siteId, Guid.NewGuid(), newMenuName, newMenuItemText, newMenuItemLocalisationText);

            _sut.Update(menuToUpdate);

            var updatedMenu = _sut.GetById(_siteId, _menuId1);
            var updatedMenuItem = updatedMenu.MenuItems.FirstOrDefault();
            var updatedMenuItemLocalisation = updatedMenuItem.MenuItemLocalisations.FirstOrDefault();

            Assert.AreEqual(newMenuName, updatedMenu.Name);
            Assert.AreEqual(newMenuItemText, updatedMenuItem.Text);
            Assert.AreEqual(newMenuItemLocalisationText, updatedMenuItemLocalisation.Text);
        }
    }
}
