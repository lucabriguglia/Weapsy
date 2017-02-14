using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using Weapsy.Domain.Themes;
using Theme = Weapsy.Data.Entities.Theme;

namespace Weapsy.Data.Tests.Facades
{
    [TestFixture]
    public class ThemeFacadeTests
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
