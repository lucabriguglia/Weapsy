using System;
using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Apps.Commands;
using Weapsy.Domain.Apps.Validators;
using Weapsy.Domain.Apps.Rules;
using Weapsy.Domain.Sites.Rules;

namespace Weapsy.Domain.Tests.Apps.Validators
{
    [TestFixture]
    public class CreateAppValidatorTests
    {
        private CreateApp _command;
        private CreateAppValidator _validator;
        private Mock<IAppRules> _appRulesMock;
        private Mock<ISiteRules> _siteRulesMock;

        [SetUp]
        public void SetUp()
        {
            _command = new CreateApp
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Description = "Description",
                Folder = "Folder"
            };

            _appRulesMock = new Mock<IAppRules>();
            _appRulesMock.Setup(x => x.IsAppIdUnique(_command.Id)).Returns(true);

            _validator = new CreateAppValidator(_appRulesMock.Object);
        }

        [Test]
        public void Should_have_validation_error_when_app_id_already_exists()
        {
            _appRulesMock = new Mock<IAppRules>();
            _appRulesMock.Setup(x => x.IsAppIdUnique(_command.Id)).Returns(false);
            _validator = new CreateAppValidator(_appRulesMock.Object);
            _validator.ShouldHaveValidationErrorFor(x => x.Id, _command);
        }
    }
}
