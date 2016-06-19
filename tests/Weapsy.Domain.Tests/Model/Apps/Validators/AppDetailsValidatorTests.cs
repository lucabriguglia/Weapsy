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
    public class AppDetailsValidatorTests
    {
        private AppDetails command;
        private AppDetailsValidator<AppDetails> validator;
        private Mock<IAppRules> appRulesMock;
        private Mock<ISiteRules> siteRulesMock;

        [SetUp]
        public void SetUp()
        {
            command = new AppDetails
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Description = "Description",
                Folder = "Folder"
            };

            appRulesMock = new Mock<IAppRules>();
            appRulesMock.Setup(x => x.IsAppIdUnique(command.Id)).Returns(true);

            validator = new AppDetailsValidator<AppDetails>(appRulesMock.Object);
        }

        [Test]
        public void Should_have_validation_error_when_app_name_is_empty()
        {
            command.Name = string.Empty;
            validator.ShouldHaveValidationErrorFor(x => x.Name, command);
        }

        [Test]
        public void Should_have_validation_error_when_app_name_is_too_long()
        {
            var name = "";
            for (int i = 0; i < 101; i++) name += i;
            command.Name = name;
            validator.ShouldHaveValidationErrorFor(x => x.Name, command);
        }

        [Test]
        public void Should_have_validation_error_when_app_name_is_not_unique()
        {
            appRulesMock = new Mock<IAppRules>();
            appRulesMock.Setup(x => x.IsAppNameUnique(command.Name, Guid.Empty)).Returns(false);
            validator = new AppDetailsValidator<AppDetails>(appRulesMock.Object);
            validator.ShouldHaveValidationErrorFor(x => x.Name, command);
        }

        [Test]
        public void Should_have_validation_error_when_app_description_is_too_long()
        {
            var description = "";
            for (int i = 0; i < 4001; i++) description += i;
            command.Description = description;
            validator.ShouldHaveValidationErrorFor(x => x.Description, command);
        }

        [Test]
        public void Should_have_validation_error_when_app_folder_is_empty()
        {
            command.Folder = string.Empty;
            validator.ShouldHaveValidationErrorFor(x => x.Folder, command);
        }

        [Test]
        public void Should_have_validation_error_when_app_folder_is_too_long()
        {
            var folder = "";
            for (int i = 0; i < 101; i++) folder += i;
            command.Folder = folder;
            validator.ShouldHaveValidationErrorFor(x => x.Folder, command);
        }

        [Test]
        public void Should_have_validation_error_when_app_folder_is_not_unique()
        {
            appRulesMock = new Mock<IAppRules>();
            appRulesMock.Setup(x => x.IsAppFolderUnique(command.Folder, Guid.Empty)).Returns(false);
            validator = new AppDetailsValidator<AppDetails>(appRulesMock.Object);
            validator.ShouldHaveValidationErrorFor(x => x.Folder, command);
        }

        [Test]
        public void Should_have_validation_error_when_app_folder_is_not_valid()
        {
            appRulesMock = new Mock<IAppRules>();
            appRulesMock.Setup(x => x.IsAppFolderValid(command.Folder)).Returns(false);
            validator = new AppDetailsValidator<AppDetails>(appRulesMock.Object);
            validator.ShouldHaveValidationErrorFor(x => x.Folder, command);
        }
    }
}
