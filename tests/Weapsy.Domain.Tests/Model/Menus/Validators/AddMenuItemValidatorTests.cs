using System;
using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Model.Menus.Validators;
using Weapsy.Domain.Model.Menus.Commands;
using Weapsy.Domain.Model.Menus;
using System.Collections.Generic;
using FluentValidation;
using Weapsy.Domain.Model.Pages.Rules;
using Weapsy.Domain.Model.Languages.Rules;
using Weapsy.Domain.Model.Sites.Rules;

namespace Weapsy.Domain.Tests.Menus.Validators
{
    [TestFixture]
    public class AddMenuItemValidatorTests
    {
        [Test]
        public void Should_have_validation_error_if_menu_item_id_is_empty()
        {
            var addMenuItem = new AddMenuItem
            {
                MenuItemId = Guid.Empty
            };

            var pageRulesMock = new Mock<IPageRules>();
            var languageRulesMock = new Mock<ILanguageRules>();
            var localisationValidatorMock = new Mock<IValidator<MenuItemDetails.MenuItemLocalisation>>();
            var siteRulesMock = new Mock<ISiteRules>();

            var validator = new AddMenuItemValidator(siteRulesMock.Object, pageRulesMock.Object, languageRulesMock.Object, localisationValidatorMock.Object);

            validator.ShouldHaveValidationErrorFor(x => x.MenuItemId, addMenuItem);
        }
    }
}
