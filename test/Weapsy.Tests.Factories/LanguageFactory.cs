using System;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Weapsy.Domain.Languages;
using Weapsy.Domain.Languages.Commands;

namespace Weapsy.Tests.Factories
{
    public static class LanguageFactory
    {
        public static Language Language()
        {
            return Language(Guid.NewGuid(), Guid.NewGuid(), "My Language", "aa-bb", "url");
        }

        public static Language Language(Guid siteId, Guid id, string name, string cultureName, string url)
        {
            var command = new CreateLanguage
            {
                SiteId = siteId,
                Id = id,
                Name = name,
                CultureName = cultureName,
                Url = url
            };

            var validatorMock = new Mock<IValidator<CreateLanguage>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            var sortOrderGeneratorMock = new Mock<ILanguageSortOrderGenerator>();
            sortOrderGeneratorMock.Setup(x => x.GenerateNextSortOrder(command.SiteId)).Returns(2);

            return Domain.Languages.Language.CreateNew(command, validatorMock.Object, sortOrderGeneratorMock.Object);
        }
    }
}
