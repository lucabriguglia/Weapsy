using System;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Weapsy.Domain.Sites;
using Site = Weapsy.Data.Entities.Site;

namespace Weapsy.Data.Tests.Reporting
{
    [TestFixture]
    public class SiteHandlersTests
    {
        private DbContextOptions<WeapsyDbContext> _contextOptions;
        private Guid _siteId;

        [SetUp]
        public void Setup()
        {
            _contextOptions = DbContextShared.CreateContextOptions();

            using (var context = new WeapsyDbContext(_contextOptions))
            {
                _siteId = Guid.NewGuid();

                context.Sites.AddRange(
                    new Site
                    {
                        Id = _siteId,
                        Name = "Site 1",
                        Status = SiteStatus.Active
                    }
                );

                context.SaveChanges();
            }
        }
    }
}
