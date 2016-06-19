using System;
using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Model.Apps.Commands;
using Weapsy.Domain.Model.Apps.Validators;
using Weapsy.Domain.Model.Apps.Rules;
using Weapsy.Domain.Model.Sites.Rules;

namespace Weapsy.Domain.Tests.Apps.Validators
{
    [TestFixture]
    public class CreateAppValidatorTests
    {
        private CreateApp command;
        private CreateAppValidator validator;
        private Mock<IAppRules> appRulesMock;
        private Mock<ISiteRules> siteRulesMock;

        [SetUp]
        public void SetUp()
        {
            command = new CreateApp
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Description = "Description",
                Folder = "Folder"
            };

            appRulesMock = new Mock<IAppRules>();
            appRulesMock.Setup(x => x.IsAppIdUnique(command.Id)).Returns(true);

            validator = new CreateAppValidator(appRulesMock.Object);
        }

        [Test]
        public void Should_have_validation_error_when_app_id_is_empty()
        {
            command.Id = Guid.Empty;
            validator.ShouldHaveValidationErrorFor(x => x.Id, command);
        }

        [Test]
        public void Should_have_validation_error_when_app_id_already_exists()
        {
            appRulesMock = new Mock<IAppRules>();
            appRulesMock.Setup(x => x.IsAppIdUnique(command.Id)).Returns(false);
            validator = new CreateAppValidator(appRulesMock.Object);
            validator.ShouldHaveValidationErrorFor(x => x.Id, command);
        }
    }
}
