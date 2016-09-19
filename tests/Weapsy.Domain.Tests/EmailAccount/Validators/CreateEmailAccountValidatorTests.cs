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
    public class CreateEmailAccountValidatorTests
    {
        private CreateEmailAccount _command;
        private CreateEmailAccountValidator _validator;
        private Mock<IEmailAccountRules> _emailAccountRulesMock;
        private Mock<ISiteRules> _siteRulesMock;

        [SetUp]
        public void SetUp()
        {
            _command = new CreateEmailAccount
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

            _validator = new CreateEmailAccountValidator(_emailAccountRulesMock.Object, _siteRulesMock.Object);
        }

        [Test]
        public void Should_have_validation_error_when_email_account_id_is_empty()
        {
            _command.Id = Guid.Empty;
            _validator.ShouldHaveValidationErrorFor(x => x.Id, _command);
        }

        [Test]
        public void Should_have_validation_error_when_email_account_id_already_exists()
        {
            _emailAccountRulesMock = new Mock<IEmailAccountRules>();
            _emailAccountRulesMock.Setup(x => x.IsEmailAccountIdUnique(_command.Id)).Returns(false);
            _validator = new CreateEmailAccountValidator(_emailAccountRulesMock.Object, _siteRulesMock.Object);
            _validator.ShouldHaveValidationErrorFor(x => x.Id, _command);
        }
    }
}
