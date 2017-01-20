using System;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Weapsy.Data.Providers.MSSQL;
using Weapsy.Domain.Sites;
using Weapsy.Tests.Shared;
using Site = Weapsy.Data.Entities.Site;

namespace Weapsy.Reporting.Data.Tests.Facades
{
    [TestFixture]
    public class SiteFacadeTests
    {
        private DbContextOptions<MSSQLDbContext> _contextOptions;
        private Guid _siteId;

        [SetUp]
        public void Setup()
        {
            _contextOptions = DbContextShared.CreateContextOptions();

            using (var context = new MSSQLDbContext(_contextOptions))
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
