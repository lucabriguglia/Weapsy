using System;
using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Model.EmailAccounts.Commands;
using Weapsy.Domain.Model.EmailAccounts.Validators;
using Weapsy.Domain.Model.EmailAccounts.Rules;
using Weapsy.Domain.Model.Sites.Rules;

namespace Weapsy.Domain.Tests.EmailAccounts.Validators
{
    [TestFixture]
    public class EmailAccountDetailsValidatorTests
    {
        private EmailAccountDetails command;
        private EmailAccountDetailsValidator<EmailAccountDetails> validator;
        private Mock<IEmailAccountRules> emailAccountRulesMock;
        private Mock<ISiteRules> siteRulesMock;

        [SetUp]
        public void SetUp()
        {
            command = new EmailAccountDetails
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

            emailAccountRulesMock = new Mock<IEmailAccountRules>();
            emailAccountRulesMock.Setup(x => x.IsEmailAccountIdUnique(command.Id)).Returns(true);

            siteRulesMock = new Mock<ISiteRules>();
            siteRulesMock.Setup(x => x.DoesSiteExist(command.SiteId)).Returns(true);

            validator = new EmailAccountDetailsValidator<EmailAccountDetails>(emailAccountRulesMock.Object, siteRulesMock.Object);
        }

        [Test]
        public void Should_have_validation_error_when_site_id_is_empty()
        {
            command.SiteId = Guid.Empty;
            validator.ShouldHaveValidationErrorFor(x => x.SiteId, command);
        }

        [Test]
        public void Should_have_validation_error_when_site_does_not_exist()
        {
            siteRulesMock = new Mock<ISiteRules>();
            siteRulesMock.Setup(x => x.DoesSiteExist(command.SiteId)).Returns(false);
            validator = new EmailAccountDetailsValidator<EmailAccountDetails>(emailAccountRulesMock.Object, siteRulesMock.Object);
            validator.ShouldHaveValidationErrorFor(x => x.SiteId, command);
        }

        [Test]
        public void Should_have_validation_error_when_email_account_address_is_empty()
        {
            command.Address = string.Empty;
            validator.ShouldHaveValidationErrorFor(x => x.Address, command);
        }

        [Test]
        public void Should_have_validation_error_when_email_account_address_is_too_long()
        {
            var address = "";
            for (int i = 0; i < 251; i++) address += i;
            command.Address = address;
            validator.ShouldHaveValidationErrorFor(x => x.Address, command);
        }

        [Test]
        public void Should_have_validation_error_when_email_account_address_is_not_valid()
        {
            command.Address = "address";
            validator.ShouldHaveValidationErrorFor(x => x.Address, command);
        }

        [Test]
        public void Should_have_validation_error_when_email_account_address_is_not_unique()
        {
            emailAccountRulesMock = new Mock<IEmailAccountRules>();
            emailAccountRulesMock.Setup(x => x.IsEmailAccountAddressUnique(command.SiteId, command.Address, Guid.Empty)).Returns(false);
            validator = new EmailAccountDetailsValidator<EmailAccountDetails>(emailAccountRulesMock.Object, siteRulesMock.Object);
            validator.ShouldHaveValidationErrorFor(x => x.Address, command);
        }

        [Test]
        public void Should_have_validation_error_when_email_account_display_name_is_too_long()
        {
            var displayName = "";
            for (int i = 0; i < 101; i++) displayName += i;
            command.DisplayName = displayName;
            validator.ShouldHaveValidationErrorFor(x => x.DisplayName, command);
        }

        [Test]
        public void Should_have_validation_error_when_email_account_host_is_empty()
        {
            command.Host = string.Empty;
            validator.ShouldHaveValidationErrorFor(x => x.Host, command);
        }

        [Test]
        public void Should_have_validation_error_when_email_account_host_is_too_long()
        {
            var host = "";
            for (int i = 0; i < 251; i++) host += i;
            command.Host = host;
            validator.ShouldHaveValidationErrorFor(x => x.Host, command);
        }

        [Test]
        public void Should_have_validation_error_when_email_account_port_is_empty()
        {
            command.Port = default(int);
            validator.ShouldHaveValidationErrorFor(x => x.Port, command);
        }

        [Test]
        public void Should_have_validation_error_when_email_account_username_is_empty_and_default_credentials_is_false()
        {
            command.DefaultCredentials = false;
            command.UserName = string.Empty;
            validator.ShouldHaveValidationErrorFor(x => x.UserName, command);
        }

        [Test]
        public void Should_have_validation_error_when_email_account_username_is_too_long_and_default_credentials_is_false()
        {
            var username = "";
            for (int i = 0; i < 251; i++) username += i;
            command.DefaultCredentials = false;
            command.UserName = username;
            validator.ShouldHaveValidationErrorFor(x => x.UserName, command);
        }

        [Test]
        public void Should_have_validation_error_when_email_account_password_is_empty_and_default_credentials_is_false()
        {
            command.DefaultCredentials = false;
            command.Password = string.Empty;
            validator.ShouldHaveValidationErrorFor(x => x.Password, command);
        }

        [Test]
        public void Should_have_validation_error_when_email_account_password_is_too_long_and_default_credentials_is_false()
        {
            var password = "";
            for (int i = 0; i < 251; i++) password += i;
            command.DefaultCredentials = false;
            command.Password = password;
            validator.ShouldHaveValidationErrorFor(x => x.Password, command);
        }
    }
}
