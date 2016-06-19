using System;
using System.Linq;
using NUnit.Framework;
using Moq;
using FluentValidation;
using FluentValidation.Results;
using Weapsy.Domain.Model.EmailAccounts;
using Weapsy.Domain.Model.EmailAccounts.Commands;
using Weapsy.Domain.Model.EmailAccounts.Events;

namespace Weapsy.Domain.Tests.EmailAccounts
{
    [TestFixture]
    public class UpdateEmailAccountDetailsTests
    {
        private UpdateEmailAccountDetails command;
        private Mock<IValidator<UpdateEmailAccountDetails>> validatorMock;
        private EmailAccount emailAccount;
        private EmailAccountDetailsUpdated @event;

        [SetUp]
        public void Setup()
        {
            command = new UpdateEmailAccountDetails
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
            validatorMock = new Mock<IValidator<UpdateEmailAccountDetails>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());
            emailAccount = new EmailAccount();
            emailAccount.UpdateDetails(command, validatorMock.Object);
            @event = emailAccount.Events.OfType<EmailAccountDetailsUpdated>().SingleOrDefault();
        }

        [Test]
        public void Should_validate_command()
        {
            validatorMock.Verify(x => x.Validate(command));
        }

        [Test]
        public void Should_set_address()
        {
            Assert.AreEqual(command.Address, emailAccount.Address);
        }

        [Test]
        public void Should_set_display_name()
        {
            Assert.AreEqual(command.DisplayName, emailAccount.DisplayName);
        }

        [Test]
        public void Should_set_host()
        {
            Assert.AreEqual(command.Host, emailAccount.Host);
        }

        [Test]
        public void Should_set_port()
        {
            Assert.AreEqual(command.Port, emailAccount.Port);
        }

        [Test]
        public void Should_set_username()
        {
            Assert.AreEqual(command.UserName, emailAccount.UserName);
        }

        [Test]
        public void Should_set_password()
        {
            Assert.AreEqual(command.Password, emailAccount.Password);
        }

        [Test]
        public void Should_set_default_credentials()
        {
            Assert.AreEqual(command.DefaultCredentials, emailAccount.DefaultCredentials);
        }

        [Test]
        public void Should_set_ssl()
        {
            Assert.AreEqual(command.Ssl, emailAccount.Ssl);
        }

        [Test]
        public void Should_add_email_account_details_updated_event()
        {
            Assert.IsNotNull(@event);
        }

        [Test]
        public void Should_set_id_in_email_account_details_updated_event()
        {
            Assert.AreEqual(emailAccount.Id, @event.AggregateRootId);
        }

        [Test]
        public void Should_set_site_id_in_email_account_details_updated_event()
        {
            Assert.AreEqual(emailAccount.SiteId, @event.SiteId);
        }

        [Test]
        public void Should_set_address_in_email_account_details_updated_event()
        {
            Assert.AreEqual(emailAccount.Address, @event.Address);
        }

        [Test]
        public void Should_set_display_name_in_email_account_details_updated_event()
        {
            Assert.AreEqual(emailAccount.DisplayName, @event.DisplayName);
        }

        [Test]
        public void Should_set_host_in_email_account_details_updated_event()
        {
            Assert.AreEqual(emailAccount.Host, @event.Host);
        }

        [Test]
        public void Should_set_port_in_email_account_details_updated_event()
        {
            Assert.AreEqual(emailAccount.Port, @event.Port);
        }

        [Test]
        public void Should_set_username_in_email_account_details_updated_event()
        {
            Assert.AreEqual(emailAccount.UserName, @event.UserName);
        }

        [Test]
        public void Should_set_password_in_email_account_details_updated_event()
        {
            Assert.AreEqual(emailAccount.Password, @event.Password);
        }

        [Test]
        public void Should_set_default_credentials_in_email_account_details_updated_event()
        {
            Assert.AreEqual(emailAccount.DefaultCredentials, @event.DefaultCredentials);
        }

        [Test]
        public void Should_set_ssl_in_email_account_details_updated_event()
        {
            Assert.AreEqual(emailAccount.Ssl, @event.Ssl);
        }
    }
}
