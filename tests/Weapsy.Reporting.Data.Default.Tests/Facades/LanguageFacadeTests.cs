using System;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using Weapsy.Data;
using Weapsy.Domain.Languages;
using Weapsy.Infrastructure.Caching;
using Weapsy.Reporting.Data.Default.Languages;
using Weapsy.Tests.Shared;
using Language = Weapsy.Data.Entities.Language;

namespace Weapsy.Reporting.Data.Default.Tests.Facades
{
    [TestFixture]
    public class LanguageFacadeTests
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
        public void Should_return_all_models_for_admin()
        {
            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var facade = new LanguageFacade(DbContextShared.CreateNewContextFactory(context), new Mock<ICacheManager>().Object, Shared.CreateNewMapper());
                var models = facade.GetAllForAdmin(_siteId);
                Assert.IsNotEmpty(models);
            }
        }

        [Test]
        public void Should_return_model_for_admin()
        {
            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var facade = new LanguageFacade(DbContextShared.CreateNewContextFactory(context), new Mock<ICacheManager>().Object, Shared.CreateNewMapper());
                var model = facade.GetForAdmin(_siteId, _languageId);
                Assert.NotNull(model);
            }
        }
    }
}
