using System;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Weapsy.Domain.Menus;
using Menu = Weapsy.Data.Entities.Menu;

namespace Weapsy.Data.Tests.Reporting
{
    [TestFixture]
    public class MenuHandlersTests
    {
        private DbContextOptions<WeapsyDbContext> _contextOptions;
        private Guid _siteId;
        private Guid _menuId;

        [SetUp]
        public void Setup()
        {
            _contextOptions = DbContextShared.CreateContextOptions();

            using (var context = new WeapsyDbContext(_contextOptions))
            {
                _siteId = Guid.NewGuid();
                _menuId = Guid.NewGuid();

                context.Menus.AddRange(
                    new Menu
                    {
                        SiteId = _siteId,
                        Id = _menuId,
                        Name = "Menu Name 1",
                        Status = MenuStatus.Active
                    }
                );

                context.SaveChanges();
            }
        }
    }
}
