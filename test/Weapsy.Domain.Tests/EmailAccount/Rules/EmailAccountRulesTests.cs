using System;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.EmailAccounts;
using Weapsy.Domain.EmailAccounts.Rules;

namespace Weapsy.Domain.Tests.EmailAccount.Rules
{
    [TestFixture]
    public class EmailAccountRulesTests
    {
        [Test]
        public void Should_return_false_if_email_account_id_is_not_unique()
        {
            var id = Guid.NewGuid();

            var repositoryMock = new Mock<IEmailAccountRepository>();
            repositoryMock.Setup(x => x.GetById(id)).Returns(new EmailAccounts.EmailAccount());

            var sut = new EmailAccountRules(repositoryMock.Object);

            var actual = sut.IsEmailAccountIdUnique(id);

            Assert.AreEqual(false, actual);
        }

        [Test]
        public void Should_return_true_if_email_account_id_is_unique()
        {
            var id = Guid.NewGuid();

            var repositoryMock = new Mock<IEmailAccountRepository>();
            repositoryMock.Setup(x => x.GetById(id)).Returns((EmailAccounts.EmailAccount)null);

            var sut = new EmailAccountRules(repositoryMock.Object);

            var actual = sut.IsEmailAccountIdUnique(id);

            Assert.AreEqual(true, actual);
        }

        [Test]
        public void Should_return_false_if_email_account_address_is_not_unique()
        {
            var siteId = Guid.NewGuid();
            var address = "info@mysite.com";

            var repositoryMock = new Mock<IEmailAccountRepository>();
            repositoryMock.Setup(x => x.GetByAddress(siteId, address)).Returns(new EmailAccounts.EmailAccount());

            var sut = new EmailAccountRules(repositoryMock.Object);

            var actual = sut.IsEmailAccountAddressUnique(siteId, address);

            Assert.AreEqual(false, actual);
        }

        [Test]
        public void Should_return_true_if_email_account_name_is_unique()
        {
            var siteId = Guid.NewGuid();
            var address = "info@mysite.com";

            var repositoryMock = new Mock<IEmailAccountRepository>();
            repositoryMock.Setup(x => x.GetByAddress(siteId, address)).Returns((EmailAccounts.EmailAccount)null);

            var sut = new EmailAccountRules(repositoryMock.Object);

            var actual = sut.IsEmailAccountAddressUnique(siteId, address);

            Assert.AreEqual(true, actual);
        }
    }
}
