using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using Weapsy.Data;
using Weapsy.Domain.Languages;
using Weapsy.Infrastructure.Caching;
using Weapsy.Reporting.Data.EmailAccounts;
using Weapsy.Tests.Factories;
using Weapsy.Tests.Shared;
using Weapsy.Domain.EmailAccounts;
using EmailAccount = Weapsy.Data.Entities.EmailAccount;

namespace Weapsy.Reporting.Data.Tests.Facades
{
    [TestFixture]
    public class EmailAccountFacadeTests
    {
        private DbContextOptions<WeapsyDbContext> _contextOptions;
        private Guid _siteId;
        private Guid _emailAccountId;

        [SetUp]
        public void Setup()
        {
            _contextOptions = DbContextShared.CreateContextOptions();

            using (var context = new WeapsyDbContext(_contextOptions))
            {
                _siteId = Guid.NewGuid();
                _emailAccountId = Guid.NewGuid();

                context.EmailAccounts.AddRange(
                    new EmailAccount
                    {
                        SiteId = _siteId,
                        Id = _emailAccountId,
                        Status = EmailAccountStatus.Active
                    }
                );

                context.SaveChanges();
            }
        }

        [Test]
        public void Should_return_all_models()
        {
            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var facade = new EmailAccountFacade(DbContextShared.CreateNewContextFactory(context), new Mock<ICacheManager>().Object, Shared.CreateNewMapper());
                var models = facade.GetAll(_siteId);
                Assert.IsNotEmpty(models);
            }
        }

        [Test]
        public void Should_return_model()
        {
            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var facade = new EmailAccountFacade(DbContextShared.CreateNewContextFactory(context), new Mock<ICacheManager>().Object, Shared.CreateNewMapper());
                var model = facade.Get(_siteId, _emailAccountId);
                Assert.NotNull(model);
            }
        }
    }
}
