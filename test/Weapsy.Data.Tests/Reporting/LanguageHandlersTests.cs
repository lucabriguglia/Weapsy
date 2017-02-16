using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Weapsy.Data.Reporting.Languages;
using Weapsy.Domain.Languages;
using Weapsy.Reporting.Languages.Queries;
using Language = Weapsy.Data.Entities.Language;

namespace Weapsy.Data.Tests.Reporting
{
    [TestFixture]
    public class LanguageHandlersTests
    {
        private DbContextOptions<WeapsyDbContext> _contextOptions;
        private Guid _siteId;
        private Guid _languageId;

        [SetUp]
        public void Setup()
        {
            _contextOptions = DbContextShared.CreateContextOptions();

            using (var context = new WeapsyDbContext(_contextOptions))
            {
                _siteId = Guid.NewGuid();
                _languageId = Guid.NewGuid();

                context.Languages.AddRange(
                    new Language
                    {
                        SiteId = _siteId,
                        Id = _languageId,
                        Name = "Language Name 1",
                        CultureName = "ab1",
                        Url = "ab1",
                        Status = LanguageStatus.Active
                    }
                );

                context.SaveChanges();
            }
        }

        [Test]
        public async Task Should_return_all_models_for_admin()
        {
            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var handler = new GetAllForAdminHandler(DbContextShared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                var models = await handler.RetrieveAsync(new GetAllForAdmin {SiteId = _siteId});
                Assert.IsNotEmpty(models);
            }
        }

        [Test]
        public async Task Should_return_model_for_admin()
        {
            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var handler = new GetForAdminHandler(DbContextShared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                var model = await handler.RetrieveAsync(new GetForAdmin { SiteId = _siteId, Id = _languageId});
                Assert.NotNull(model);
            }
        }
    }
}
