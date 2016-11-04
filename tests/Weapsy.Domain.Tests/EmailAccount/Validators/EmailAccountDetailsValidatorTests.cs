using System;
using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.EmailAccounts.Commands;
using Weapsy.Domain.EmailAccounts.Rules;
using Weapsy.Domain.EmailAccounts.Validators;
using Weapsy.Domain.Sites.Rules;

namespace Weapsy.Domain.Tests.EmailAccount.Validators
{
    [TestFixture]
    public class EmailAccountDetailsValidatorTests
    {
        private EmailAccountDetails _command;
        private EmailAccountDetailsValidator<EmailAccountDetails> _validator;
        private Mock<IEmailAccountRules> _emailAccountRulesMock;
        private Mock<ISiteRules> _siteRulesMock;

        [SetUp]
        public void SetUp()
        {
            _command = new EmailAccountDetails
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

            _emailAccountRulesMock = new Mock<IEmailAccountRules>();
            _emailAccountRulesMock.Setup(x => x.IsEmailAccountIdUnique(_command.Id)).Returns(true);

            _siteRulesMock = new Mock<ISiteRules>();
            _siteRulesMock.Setup(x => x.DoesSiteExist(_command.SiteId)).Returns(true);

            _validator = new EmailAccountDetailsValidator<EmailAccountDetails>(_emailAccountRulesMock.Object, _siteRulesMock.Object);
        }

        [Test]
        public void Should_have_validation_error_when_email_account_address_is_empty()
        {
            _command.Address = string.Empty;
            _validator.ShouldHaveValidationErrorFor(x => x.Address, _command);
        }

        [Test]
        public void Should_have_validation_error_when_email_account_address_is_too_long()
        {
            var address = "";
            for (int i = 0; i < 251; i++) address += i;
            _command.Address = address;
            _validator.ShouldHaveValidationErrorFor(x => x.Address, _command);
        }

        [Test]
        public void Should_have_validation_error_when_email_account_address_is_not_valid()
        {
            _command.Address = "address";
            _validator.ShouldHaveValidationErrorFor(x => x.Address, _command);
        }

        [Test]
        public void Should_have_validation_error_when_email_account_address_is_not_unique()
        {
            _emailAccountRulesMock = new Mock<IEmailAccountRules>();
            _emailAccountRulesMock.Setup(x => x.IsEmailAccountAddressUnique(_command.SiteId, _command.Address, Guid.Empty)).Returns(false);
            _validator = new EmailAccountDetailsValidator<EmailAccountDetails>(_emailAccountRulesMock.Object, _siteRulesMock.Object);
            _validator.ShouldHaveValidationErrorFor(x => x.Address, _command);
        }

        [Test]
        public void Should_have_validation_error_when_email_account_display_name_is_too_long()
        {
            var displayName = "";
            for (int i = 0; i < 101; i++) displayName += i;
            _command.DisplayName = displayName;
            _validator.ShouldHaveValidationErrorFor(x => x.DisplayName, _command);
        }

        [Test]
        public void Should_have_validation_error_when_email_account_host_is_empty()
        {
            _command.Host = string.Empty;
            _validator.ShouldHaveValidationErrorFor(x => x.Host, _command);
        }

        [Test]
        public void Should_have_validation_error_when_email_account_host_is_too_long()
        {
            var host = "";
            for (int i = 0; i < 251; i++) host += i;
            _command.Host = host;
            _validator.ShouldHaveValidationErrorFor(x => x.Host, _command);
        }

        [Test]
        public void Should_have_validation_error_when_email_account_port_is_empty()
        {
            _command.Port = default(int);
            _validator.ShouldHaveValidationErrorFor(x => x.Port, _command);
        }

        [Test]
        public void Should_have_validation_error_when_email_account_username_is_empty_and_default_credentials_is_false()
        {
            _command.DefaultCredentials = false;
            _command.UserName = string.Empty;
            _validator.ShouldHaveValidationErrorFor(x => x.UserName, _command);
        }

        [Test]
        public void Should_have_validation_error_when_email_account_username_is_too_long_and_default_credentials_is_false()
        {
            var username = "";
            for (int i = 0; i < 251; i++) username += i;
            _command.DefaultCredentials = false;
            _command.UserName = username;
            _validator.ShouldHaveValidationErrorFor(x => x.UserName, _command);
        }

        [Test]
        public void Should_have_validation_error_when_email_account_password_is_empty_and_default_credentials_is_false()
        {
            _command.DefaultCredentials = false;
            _command.Password = string.Empty;
            _validator.ShouldHaveValidationErrorFor(x => x.Password, _command);
        }

        [Test]
        public void Should_have_validation_error_when_email_account_password_is_too_long_and_default_credentials_is_false()
        {
            var password = "";
            for (int i = 0; i < 251; i++) password += i;
            _command.DefaultCredentials = false;
            _command.Password = password;
            _validator.ShouldHaveValidationErrorFor(x => x.Password, _command);
        }
    }
}
