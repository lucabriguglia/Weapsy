using System;
using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Weapsy.Domain.Menus;
using Weapsy.Domain.Menus.Commands;

namespace Weapsy.Tests.Factories
{
    public static class MenuFactory
    {
        public static Menu Menu()
        {
            return Menu(Guid.NewGuid(), Guid.NewGuid(), "My Menu", "My Item", "My Item Localised");
        }

        public static Menu Menu(Guid siteId, Guid id, string name, string itemText, string itemTextLocalised, Guid menuItemId = new Guid(), Guid languageId = new Guid())
        {
            if (menuItemId == Guid.Empty)
                menuItemId = Guid.NewGuid();

            if (languageId == Guid.Empty)
                languageId = Guid.NewGuid();

            var createCommand = new CreateMenu
            {
                SiteId = siteId,
                Id = id,
                Name = name
            };

            var createValidatorMock = new Mock<IValidator<CreateMenu>>();
            createValidatorMock.Setup(x => x.Validate(createCommand)).Returns(new ValidationResult());

            var menu = Domain.Menus.Menu.CreateNew(createCommand, createValidatorMock.Object);

            var addItemCommand = new AddMenuItem
            {
                SiteId = menu.SiteId,
                MenuId = menu.Id,
                MenuItemId = menuItemId,
                Type = MenuItemType.Link,
                PageId = Guid.NewGuid(),
                Link = "link",
                Text = itemText,
                Title = "Title",
                MenuItemLocalisations = new List<MenuItemLocalisation>
                {
                    new MenuItemLocalisation
                    {
                        LanguageId = languageId,
                        Text = itemTextLocalised,
                        Title = "Title 1"
                    }
                }
            };

            var addItemValidatorMock = new Mock<IValidator<AddMenuItem>>();
            addItemValidatorMock.Setup(x => x.Validate(addItemCommand)).Returns(new ValidationResult());

            menu.AddMenuItem(addItemCommand, addItemValidatorMock.Object);

            return menu;
        }
    }
}
