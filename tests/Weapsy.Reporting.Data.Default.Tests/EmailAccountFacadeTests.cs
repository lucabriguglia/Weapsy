using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using Weapsy.Core.Caching;
using Weapsy.Domain.EmailAccounts;
using Weapsy.Reporting.Data.Default.EmailAccounts;
using Weapsy.Reporting.EmailAccounts;

namespace Weapsy.Reporting.Data.Default.Tests
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

            var repositoryMock = new Mock<IEmailAccountRepository>();
            repositoryMock.Setup(x => x.GetById(_siteId, _emailAccountId)).Returns(new EmailAccount());
            repositoryMock.Setup(x => x.GetAll(_siteId)).Returns(new List<EmailAccount>() { new EmailAccount(), new EmailAccount() });

            var cacheManagerMock = new Mock<ICacheManager>();

            var mapperMock = new Mock<AutoMapper.IMapper>();
            mapperMock.Setup(x => x.Map<EmailAccountModel>(It.IsAny<EmailAccount>())).Returns(new EmailAccountModel());

            _sut = new EmailAccountFacade(repositoryMock.Object, cacheManagerMock.Object, mapperMock.Object);
        }

        [Test]
        public void Should_return_all_models()
        {
            var model = _sut.GetAll(_siteId);
            Assert.IsNotEmpty(model);
        }

        [Test]
        public void Should_return_model()
        {
            var model = _sut.Get(_siteId, _emailAccountId);
            Assert.NotNull(model);
        }
    }
}
