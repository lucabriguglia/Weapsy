using System;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using Weapsy.Data.Reporting.EmailAccounts;
using Weapsy.Domain.EmailAccounts;
using Weapsy.Infrastructure.Caching;
using EmailAccount = Weapsy.Data.Entities.EmailAccount;

namespace Weapsy.Data.Tests.Reporting
{
    [TestFixture]
    public class EmailAccountHandlersTests
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
