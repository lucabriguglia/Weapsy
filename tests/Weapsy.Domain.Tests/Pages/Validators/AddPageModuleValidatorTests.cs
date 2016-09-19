using System;
using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Model.Pages.Commands;
using Weapsy.Domain.Model.Pages.Validators;
using Weapsy.Domain.Model.Modules.Rules;
using Weapsy.Domain.Model.Sites.Rules;

namespace Weapsy.Domain.Tests.Pages.Validators
{
    [TestFixture]
    public class AddPageModuleValidatorTests
    {
        //[Test]
        //public void Should_have_error_when_id_is_empty()
        //{
        //    var moduleRules = new Mock<IModuleRules>();
        //    var validator = new AddPageModuleValidator(moduleRules.Object);

        //    validator.ShouldHaveValidationErrorFor(x => x.Id, new AddPageModule
        //    {
        //        SiteId = Guid.NewGuid(),
        //        PageId = Guid.NewGuid(),
        //        ModuleId = Guid.NewGuid(),
        //        Id = Guid.Empty,
        //        Title = "Title",
        //        Zone = "Zone"
        //    });
        //}

        //[Test]
        //public void Should_have_error_when_id_is_not_unique()
        //{
        //    var moduleRules = new Mock<IModuleRules>();
        //    moduleRules.Setup(x => x.IsModuleIdUnique(It.IsAny<Guid>())).Returns(false);
        //    var validator = new AddPageModuleValidator(moduleRules.Object);

        //    validator.ShouldHaveValidationErrorFor(x => x.Id, new AddPageModule
        //    {
        //        SiteId = Guid.NewGuid(),
        //        PageId = Guid.NewGuid(),
        //        ModuleId = Guid.NewGuid(),
        //        Id = Guid.NewGuid(),
        //        Title = "Title",
        //        Zone = "Zone"
        //    });
        //}

        [Test]
        public void Should_have_validation_error_when_site_id_is_empty()
        {
            var command = new AddPageModule
            {
                SiteId = Guid.Empty
            };

            var moduleRulesMock = new Mock<IModuleRules>();
            var siteRulesMock = new Mock<ISiteRules>();
            var validator = new AddPageModuleValidator(siteRulesMock.Object, moduleRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.SiteId, command);
        }

        [Test]
        public void Should_have_validation_error_when_site_does_not_exist()
        {
            var command = new AddPageModule
            {
                SiteId = Guid.NewGuid()
            };

            var moduleRulesMock = new Mock<IModuleRules>();
            var siteRulesMock = new Mock<ISiteRules>();
            siteRulesMock.Setup(x => x.DoesSiteExist(command.SiteId)).Returns(false);

            var validator = new AddPageModuleValidator(siteRulesMock.Object, moduleRulesMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.SiteId, command);
        }

        [Test]
        public void Should_have_error_when_module_id_is_empty()
        {
            var moduleRules = new Mock<IModuleRules>();
            var siteRulesMock = new Mock<ISiteRules>();
            var validator = new AddPageModuleValidator(siteRulesMock.Object, moduleRules.Object);

            validator.ShouldHaveValidationErrorFor(x => x.ModuleId, new AddPageModule
            {
                SiteId = Guid.NewGuid(),
                PageId = Guid.NewGuid(),
                ModuleId = Guid.Empty,
                Id = Guid.NewGuid(),
                Title = "Title",
                Zone = "Zone"
            });
        }

        [Test]
        public void Should_have_error_when_module_does_not_exist()
        {
            var moduleRules = new Mock<IModuleRules>();
            moduleRules.Setup(x => x.DoesModuleExist(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(false);
            var siteRulesMock = new Mock<ISiteRules>();
            var validator = new AddPageModuleValidator(siteRulesMock.Object, moduleRules.Object);

            validator.ShouldHaveValidationErrorFor(x => x.ModuleId, new AddPageModule
            {
                SiteId = Guid.NewGuid(),
                PageId = Guid.NewGuid(),
                ModuleId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Title = "Title",
                Zone = "Zone"
            });
        }

        [Test]
        public void Should_have_error_when_title_is_too_long()
        {
            var moduleRules = new Mock<IModuleRules>();
            var siteRulesMock = new Mock<ISiteRules>();
            var validator = new AddPageModuleValidator(siteRulesMock.Object, moduleRules.Object);

            var title = "";
            for (int i = 0; i < 101; i++) title += i;

            validator.ShouldHaveValidationErrorFor(x => x.Title, new AddPageModule
            {
                SiteId = Guid.NewGuid(),
                PageId = Guid.NewGuid(),
                ModuleId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Title = title,
                Zone = "Zone"
            });
        }

        [Test]
        public void Should_have_error_when_zone_is_empty()
        {
            var moduleRules = new Mock<IModuleRules>();
            var siteRulesMock = new Mock<ISiteRules>();
            var validator = new AddPageModuleValidator(siteRulesMock.Object, moduleRules.Object);

            validator.ShouldHaveValidationErrorFor(x => x.Zone, new AddPageModule
            {
                SiteId = Guid.NewGuid(),
                PageId = Guid.NewGuid(),
                ModuleId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Title = "Title",
                Zone = string.Empty
            });
        }

        [Test]
        public void Should_have_error_when_zone_is_too_long()
        {
            var moduleRules = new Mock<IModuleRules>();
            var siteRulesMock = new Mock<ISiteRules>();
            var validator = new AddPageModuleValidator(siteRulesMock.Object, moduleRules.Object);

            var zone = "";
            for (int i = 0; i < 101; i++) zone += i;

            validator.ShouldHaveValidationErrorFor(x => x.Zone, new AddPageModule
            {
                SiteId = Guid.NewGuid(),
                PageId = Guid.NewGuid(),
                ModuleId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Title = "Title",
                Zone = zone
            });
        }
    }
}
