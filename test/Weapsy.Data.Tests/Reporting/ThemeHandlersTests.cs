using System;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Weapsy.Domain.Themes;
using Theme = Weapsy.Data.Entities.Theme;

namespace Weapsy.Data.Tests.Reporting
{
    [TestFixture]
    public class ThemeHandlersTests
    {
        private DbContextOptions<WeapsyDbContext> _contextOptions;
        private Guid _themeId;

        [SetUp]
        public void Setup()
        {
            _contextOptions = DbContextShared.CreateContextOptions();

            using (var context = new WeapsyDbContext(_contextOptions))
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
