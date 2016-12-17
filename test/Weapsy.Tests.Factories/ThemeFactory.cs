using System;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Weapsy.Domain.Themes;
using Weapsy.Domain.Themes.Commands;

namespace Weapsy.Tests.Factories
{
    public static class ThemeFactory
    {
        public static Theme Theme()
        {
            return Theme(Guid.NewGuid(), "My Theme Name", "My Theme Description", "My Theme Folder");
        }

        public static Theme Theme(Guid id, string name, string description, string folder)
        {
            var command = new CreateTheme
            {
                Id = id,
                Name = name,
                Description = description,
                Folder = folder
            };

            var validatorMock = new Mock<IValidator<CreateTheme>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            var sortOrderGeneratorMock = new Mock<IThemeSortOrderGenerator>();
            sortOrderGeneratorMock.Setup(x => x.GenerateNextSortOrder()).Returns(2);

            return Domain.Themes.Theme.CreateNew(command, validatorMock.Object, sortOrderGeneratorMock.Object);
        }
    }
}
