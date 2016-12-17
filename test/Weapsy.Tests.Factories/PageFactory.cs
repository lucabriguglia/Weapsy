using System;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Weapsy.Domain.Pages;
using Weapsy.Domain.Pages.Commands;
using System.Collections.Generic;

namespace Weapsy.Tests.Factories
{
    public static class PageFactory
    {
        public static Page Page()
        {
            return Page(Guid.NewGuid(), Guid.NewGuid(), "My Page");
        }

        public static Page Page(Guid siteId, Guid id, string name, Guid pageModuleId = new Guid(), Guid moduleId = new Guid())
        {
            if (pageModuleId == Guid.Empty)
                pageModuleId = Guid.NewGuid();

            if (moduleId == Guid.Empty)
                moduleId = Guid.NewGuid();

            var createCommand = new CreatePage
            {
                SiteId = siteId,
                Id = id,
                Name = name,
                Url = "url",
                Title = "Title",
                MetaDescription = "Meta Description",
                MetaKeywords = "Meta Keywords",
                PageLocalisations = new List<PageLocalisation>
                {
                    new PageLocalisation
                    {
                        LanguageId = Guid.NewGuid(),
                        Url = "url",
                        Title = "Head Title",
                        MetaDescription = "Meta Description",
                        MetaKeywords = "Meta Keywords"
                    }
                }
            };

            var createValidatorMock = new Mock<IValidator<CreatePage>>();
            createValidatorMock.Setup(x => x.Validate(createCommand)).Returns(new ValidationResult());

            var page = Domain.Pages.Page.CreateNew(createCommand, createValidatorMock.Object);

            var addModuleCommand = new AddPageModule
            {
                SiteId = siteId,
                PageId = id,
                ModuleId = moduleId,
                PageModuleId = pageModuleId,
                Title = "Title",
                Zone = "Zone",
                SortOrder = 1
            };

            var addModuleValidatorMock = new Mock<IValidator<AddPageModule>>();
            addModuleValidatorMock.Setup(x => x.Validate(addModuleCommand)).Returns(new ValidationResult());

            page.AddModule(addModuleCommand, addModuleValidatorMock.Object);

            return page;
        }
    }
}
