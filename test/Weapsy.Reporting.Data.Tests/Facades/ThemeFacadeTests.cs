using System;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Weapsy.Data.Providers.MSSQL;
using Weapsy.Domain.Themes;
using Weapsy.Tests.Shared;
using Theme = Weapsy.Data.Entities.Theme;

namespace Weapsy.Reporting.Data.Tests.Facades
{
    [TestFixture]
    public class ThemeFacadeTests
    {
        private DbContextOptions<MSSQLDbContext> _contextOptions;
        private Guid _themeId;

        [SetUp]
        public void Setup()
        {
            _contextOptions = DbContextShared.CreateContextOptions();

            using (var context = new MSSQLDbContext(_contextOptions))
            {
                _themeId = Guid.NewGuid();

                context.Themes.AddRange(
                    new Theme
                    {
                        Id = _themeId,
                        Name = "Theme 1",
                        Status = ThemeStatus.Active
                    }
                );

                context.SaveChanges();
            }
        }
    }
}
