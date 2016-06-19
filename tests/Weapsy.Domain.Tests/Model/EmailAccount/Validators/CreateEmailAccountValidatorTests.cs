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
        private CreateEmailAccount command;
        private CreateEmailAccountValidator validator;
        private Mock<IEmailAccountRules> emailAccountRulesMock;
        private Mock<ISiteRules> siteRulesMock;

        [SetUp]
        public void SetUp()
        {
            command = new CreateEmailAccount
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

            validator = new CreateEmailAccountValidator(emailAccountRulesMock.Object, siteRulesMock.Object);
        }

        [Test]
        public void Should_have_validation_error_when_email_account_id_is_empty()
        {
            command.Id = Guid.Empty;
            validator.ShouldHaveValidationErrorFor(x => x.Id, command);
        }

        [Test]
        public void Should_have_validation_error_when_email_account_id_already_exists()
        {
            emailAccountRulesMock = new Mock<IEmailAccountRules>();
            emailAccountRulesMock.Setup(x => x.IsEmailAccountIdUnique(command.Id)).Returns(false);
            validator = new CreateEmailAccountValidator(emailAccountRulesMock.Object, siteRulesMock.Object);
            validator.ShouldHaveValidationErrorFor(x => x.Id, command);
        }
    }
}
