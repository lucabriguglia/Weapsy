using System;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.EmailAccounts;
using Weapsy.Domain.EmailAccounts.Commands;
using Weapsy.Domain.EmailAccounts.Events;

namespace Weapsy.Domain.Tests.EmailAccount
{
    [TestFixture]
    public class DeleteEmailAccountTests
    {
        private EmailAccounts.EmailAccount _emailAccount;
        private Mock<IValidator<DeleteEmailAccount>> _validatorMock;
        private DeleteEmailAccount _command;
        private EmailAccountDeleted _event;

        [SetUp]
        public void SetUp()
        {
            var createEmailAccountCommand = new CreateEmailAccount
            {
                SiteId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Address = "info@mysite.com",
                DisplayName = "My Site",
                Host = "host",
                Port = 25,
                UserName = "username",
                Password = "password",
                DefaultCredentials = true,
                Ssl = true
            };
            var createEmailAccountValidatorMock = new Mock<IValidator<CreateEmailAccount>>();
            createEmailAccountValidatorMock.Setup(x => x.Validate(createEmailAccountCommand)).Returns(new ValidationResult());
            _emailAccount = EmailAccounts.EmailAccount.CreateNew(createEmailAccountCommand, createEmailAccountValidatorMock.Object);

            _command = new DeleteEmailAccount
            {
                SiteId = _emailAccount.SiteId,
                Id = _emailAccount.Id
            };

            _validatorMock = new Mock<IValidator<DeleteEmailAccount>>();
            _validatorMock.Setup(x => x.Validate(_command)).Returns(new ValidationResult());

            _emailAccount.Delete(_command, _validatorMock.Object);

            _event = _emailAccount.Events.OfType<EmailAccountDeleted>().SingleOrDefault();
        }

        [Test]
        public void Should_call_validator()
        {
            _validatorMock.Verify(x => x.Validate(_command));
        }

        [Test]
        public void Should_throw_exception_when_already_deleted()
        {
            Assert.Throws<Exception>(() => _emailAccount.Delete(_command, _validatorMock.Object));
        }

        [Test]
        public void Should_set_email_account_status_to_deleted()
        {
            Assert.AreEqual(true, _emailAccount.Status == EmailAccountStatus.Deleted);
        }

        [Test]
        public void Should_add_email_account_deleted_event()
        {
            Assert.IsNotNull(_event);
        }

        [Test]
        public void Should_set_id_in_email_account_deleted_event()
        {
            Assert.AreEqual(_emailAccount.Id, _event.AggregateRootId);
        }

        [Test]
        public void Should_set_site_id_in_email_account_deleted_event()
        {
            Assert.AreEqual(_emailAccount.SiteId, _event.SiteId);
        }
    }
}