using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Weapsy.Data.Reporting.EmailAccounts;
using Weapsy.Domain.EmailAccounts;
using Weapsy.Reporting.EmailAccounts.Queries;
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
        public async Task Should_return_all_models()
        {
            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var handler = new GetAllEmailAccountsHandler(DbContextShared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                var models = await handler.RetrieveAsync(new GetAllEmailAccounts {SiteId = _siteId});
                Assert.IsNotEmpty(models);
            }
        }

        [Test]
        public void Should_return_model()
        {
            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var handler = new GetEmailAccountHandler(DbContextShared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                var model = handler.RetrieveAsync(new GetEmailAccount { SiteId = _siteId, Id = _emailAccountId});
                Assert.NotNull(model);
            }
        }
    }
}
