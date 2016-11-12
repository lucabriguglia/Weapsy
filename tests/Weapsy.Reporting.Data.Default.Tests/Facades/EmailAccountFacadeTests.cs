using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.EmailAccounts;
using Weapsy.Infrastructure.Caching;
using Weapsy.Reporting.Data.Default.EmailAccounts;
using Weapsy.Reporting.EmailAccounts;
using Weapsy.Tests.Factories;

namespace Weapsy.Reporting.Data.Default.Tests.Facades
{
    [TestFixture]
    public class EmailAccountFacadeTests
    {
        private IEmailAccountFacade _sut;
        private Guid _siteId;
        private Guid _emailAccountId;

        [SetUp]
        public void Setup()
        {
            _siteId = Guid.NewGuid();
            _emailAccountId = Guid.NewGuid();

            var emailAccount = EmailAccountFactory.EmailAccount(_siteId, _emailAccountId, "info@weapsy.org");

            var repositoryMock = new Mock<IEmailAccountRepository>();
            repositoryMock.Setup(x => x.GetById(_siteId, _emailAccountId)).Returns(emailAccount);
            repositoryMock.Setup(x => x.GetAll(_siteId)).Returns(new List<EmailAccount>() { emailAccount });

            var cacheManagerMock = new Mock<ICacheManager>();

            _sut = new EmailAccountFacade(repositoryMock.Object, cacheManagerMock.Object, Shared.CreateNewMapper());
        }

        [Test]
        public void Should_return_all_models()
        {
            var models = _sut.GetAll(_siteId);
            Assert.IsNotEmpty(models);
        }

        [Test]
        public void Should_return_model()
        {
            var model = _sut.Get(_siteId, _emailAccountId);
            Assert.NotNull(model);
        }
    }
}
