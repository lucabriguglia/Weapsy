using System.Linq;
using NUnit.Framework;
using Weapsy.Domain.Model.EmailAccounts;
using Weapsy.Domain.Model.EmailAccounts.Events;
using System;
using Moq;
using Weapsy.Domain.Model.EmailAccounts.Commands;
using FluentValidation;
using FluentValidation.Results;

namespace Weapsy.Domain.Tests.EmailAccounts
{
    [TestFixture]
    public class DeleteEmailAccountTests
    {
        private EmailAccount emailAccount;
        private Mock<IValidator<DeleteEmailAccount>> validatorMock;
        private DeleteEmailAccount command;
        private EmailAccountDeleted @event;

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
            emailAccount = EmailAccount.CreateNew(createEmailAccountCommand, createEmailAccountValidatorMock.Object);

            command = new DeleteEmailAccount
            {
                SiteId = emailAccount.SiteId,
                Id = emailAccount.Id
            };

            validatorMock = new Mock<IValidator<DeleteEmailAccount>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            emailAccount.Delete(command, validatorMock.Object);

            @event = emailAccount.Events.OfType<EmailAccountDeleted>().SingleOrDefault();
        }

        [Test]
        public void Should_call_validator()
        {
            validatorMock.Verify(x => x.Validate(command));
        }

        [Test]
        public void Should_throw_exception_when_already_deleted()
        {
            Assert.Throws<Exception>(() => emailAccount.Delete(command, validatorMock.Object));
        }

        [Test]
        public void Should_set_email_account_status_to_deleted()
        {
            Assert.AreEqual(true, emailAccount.Status == EmailAccountStatus.Deleted);
        }

        [Test]
        public void Should_add_email_account_deleted_event()
        {
            Assert.IsNotNull(@event);
        }

        [Test]
        public void Should_set_id_in_email_account_deleted_event()
        {
            Assert.AreEqual(emailAccount.Id, @event.AggregateRootId);
        }

        [Test]
        public void Should_set_site_id_in_email_account_deleted_event()
        {
            Assert.AreEqual(emailAccount.SiteId, @event.SiteId);
        }
    }
}