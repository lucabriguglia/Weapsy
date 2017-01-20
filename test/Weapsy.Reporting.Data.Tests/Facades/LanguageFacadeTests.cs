using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using Weapsy.Data.Providers.MSSQL;
using Weapsy.Domain.Languages;
using Weapsy.Infrastructure.Caching;
using Weapsy.Reporting.Data.Languages;
using Weapsy.Tests.Shared;
using Language = Weapsy.Data.Entities.Language;

namespace Weapsy.Reporting.Data.Tests.Facades
{
    [TestFixture]
    public class LanguageFacadeTests
    {
        private DbContextOptions<MSSQLDbContext> _contextOptions;
        private Guid _siteId;
        private Guid _languageId;

        [SetUp]
        public void Setup()
        {
            _contextOptions = DbContextShared.CreateContextOptions();

            using (var context = new MSSQLDbContext(_contextOptions))
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
            using (var context = new MSSQLDbContext(_contextOptions))
            {
                var facade = new LanguageFacade(DbContextShared.CreateNewContextFactory(context), new Mock<ICacheManager>().Object, Shared.CreateNewMapper());
                var models = await facade.GetAllForAdminAsync(_siteId);
                Assert.IsNotEmpty(models);
            }
        }

        [Test]
        public void Should_return_model_for_admin()
        {
            using (var context = new MSSQLDbContext(_contextOptions))
            {
                var facade = new LanguageFacade(DbContextShared.CreateNewContextFactory(context), new Mock<ICacheManager>().Object, Shared.CreateNewMapper());
                var model = facade.GetForAdminAsync(_siteId, _languageId);
                Assert.NotNull(model);
            }
        }
    }
}
