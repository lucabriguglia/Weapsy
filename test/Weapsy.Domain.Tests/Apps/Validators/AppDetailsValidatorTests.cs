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
    public class AppDetailsValidatorTests
    {
        private AppDetails _command;
        private AppDetailsValidator<AppDetails> _validator;
        private Mock<IAppRules> _appRulesMock;
        private Mock<ISiteRules> _siteRulesMock;

        [SetUp]
        public void SetUp()
        {
            _command = new AppDetails
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Description = "Description",
                Folder = "Folder"
            };

            _appRulesMock = new Mock<IAppRules>();
            _appRulesMock.Setup(x => x.IsAppIdUnique(_command.Id)).Returns(true);

            _validator = new AppDetailsValidator<AppDetails>(_appRulesMock.Object);
        }

        [Test]
        public void Should_have_validation_error_when_app_name_is_empty()
        {
            _command.Name = string.Empty;
            _validator.ShouldHaveValidationErrorFor(x => x.Name, _command);
        }

        [Test]
        public void Should_have_validation_error_when_app_name_is_too_long()
        {
            var name = "";
            for (int i = 0; i < 101; i++) name += i;
            _command.Name = name;
            _validator.ShouldHaveValidationErrorFor(x => x.Name, _command);
        }

        [Test]
        public void Should_have_validation_error_when_app_name_is_not_unique()
        {
            _appRulesMock = new Mock<IAppRules>();
            _appRulesMock.Setup(x => x.IsAppNameUnique(_command.Name, Guid.Empty)).Returns(false);
            _validator = new AppDetailsValidator<AppDetails>(_appRulesMock.Object);
            _validator.ShouldHaveValidationErrorFor(x => x.Name, _command);
        }

        [Test]
        public void Should_have_validation_error_when_app_description_is_too_long()
        {
            var description = "";
            for (int i = 0; i < 4001; i++) description += i;
            _command.Description = description;
            _validator.ShouldHaveValidationErrorFor(x => x.Description, _command);
        }

        [Test]
        public void Should_have_validation_error_when_app_folder_is_empty()
        {
            _command.Folder = string.Empty;
            _validator.ShouldHaveValidationErrorFor(x => x.Folder, _command);
        }

        [Test]
        public void Should_have_validation_error_when_app_folder_is_too_long()
        {
            var folder = "";
            for (int i = 0; i < 101; i++) folder += i;
            _command.Folder = folder;
            _validator.ShouldHaveValidationErrorFor(x => x.Folder, _command);
        }

        [Test]
        public void Should_have_validation_error_when_app_folder_is_not_unique()
        {
            _appRulesMock = new Mock<IAppRules>();
            _appRulesMock.Setup(x => x.IsAppFolderUnique(_command.Folder, Guid.Empty)).Returns(false);
            _validator = new AppDetailsValidator<AppDetails>(_appRulesMock.Object);
            _validator.ShouldHaveValidationErrorFor(x => x.Folder, _command);
        }

        [Test]
        public void Should_have_validation_error_when_app_folder_is_not_valid()
        {
            _appRulesMock = new Mock<IAppRules>();
            _appRulesMock.Setup(x => x.IsAppFolderValid(_command.Folder)).Returns(false);
            _validator = new AppDetailsValidator<AppDetails>(_appRulesMock.Object);
            _validator.ShouldHaveValidationErrorFor(x => x.Folder, _command);
        }
    }
}
