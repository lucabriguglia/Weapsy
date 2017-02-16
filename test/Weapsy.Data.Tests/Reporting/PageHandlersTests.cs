using System;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Weapsy.Domain.Pages;
using Page = Weapsy.Data.Entities.Page;

namespace Weapsy.Data.Tests.Reporting
{
    [TestFixture]
    public class PageHandlersTests
    {
        private DbContextOptions<WeapsyDbContext> _contextOptions;
        private Guid _pageId;

        [SetUp]
        public void Setup()
        {
            _contextOptions = DbContextShared.CreateContextOptions();

            using (var context = new WeapsyDbContext(_contextOptions))
            {
                _pageId = Guid.NewGuid();

                context.Pages.AddRange(
                    new Page
                    {
                        Id = _pageId,
                        Name = "Page 1",
                        Status = PageStatus.Active
                    }
                );

                context.SaveChanges();
            }
        }
    }
}
