using System;
using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Pages;
using Weapsy.Domain.Pages.Commands;
using Weapsy.Domain.Pages.Handlers;

namespace Weapsy.Domain.Tests.Pages.Handlers
{
    [TestFixture]
    public class UpdatePageModuleDetailsHandlerTests
    {
        [Test]
        public void Should_throw_exception_when_page_is_not_found()
        {
            var command = new UpdatePageModuleDetails
            {
                SiteId = Guid.NewGuid(),
                PageId = Guid.NewGuid(),
                ModuleId = Guid.NewGuid(),
                Title = "Title",
                PageModuleLocalisations = new List<PageModuleLocalisation>
                {
                    new PageModuleLocalisation
                    {
                        LanguageId = Guid.NewGuid(),
                        Title = "Title"
                    },
                    new PageModuleLocalisation
                    {
                        LanguageId = Guid.NewGuid(),
                        Title = "Title"
                    }
                }
            };

            var repositoryMock = new Mock<IPageRepository>();
            repositoryMock.Setup(x => x.GetById(command.SiteId, command.PageId)).Returns((Page)null);

            var validatorMock = new Mock<IValidator<UpdatePageModuleDetails>>();

            var createPageHandler = new UpdatePageModuleDetailsHandler(repositoryMock.Object, validatorMock.Object);

            Assert.Throws<Exception>(() => createPageHandler.Handle(command));
        }

        [Test]
        public void Should_throw_validation_exception_when_validation_fails()
        {
            var page = new Page();

            var siteId = Guid.NewGuid();
            var pageId = Guid.NewGuid();
            var moduleId = Guid.NewGuid();

            var addPageModuleCommand = new AddPageModule
            {
                SiteId = siteId,
                PageId = pageId,
                ModuleId = moduleId,
                PageModuleId = Guid.NewGuid(),
                Title = "Title",
                Zone = "Zone"
            };
            var addPageModuleValidatorMock = new Mock<IValidator<AddPageModule>>();
            addPageModuleValidatorMock.Setup(x => x.Validate(addPageModuleCommand)).Returns(new ValidationResult());
            page.AddModule(addPageModuleCommand, addPageModuleValidatorMock.Object);

            var command = new UpdatePageModuleDetails
            {
                SiteId = siteId,
                PageId = pageId,
                ModuleId = moduleId,
                Title = "New Title",
                PageModuleLocalisations = new List<PageModuleLocalisation>
                {
                    new PageModuleLocalisation
                    {
                        LanguageId = Guid.NewGuid(),
                        Title = "Title"
                    },
                    new PageModuleLocalisation
                    {
                        LanguageId = Guid.NewGuid(),
                        Title = "Title"
                    }
                }
            };

            var repositoryMock = new Mock<IPageRepository>();
            repositoryMock.Setup(x => x.GetById(command.SiteId, command.PageId)).Returns(page);

            var validatorMock = new Mock<IValidator<UpdatePageModuleDetails>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Title", "Title Error") }));

            var createPageHandler = new UpdatePageModuleDetailsHandler(repositoryMock.Object, validatorMock.Object);

            Assert.Throws<Exception>(() => createPageHandler.Handle(command));
        }

        [Test]
        public void Should_validate_command_and_update_page()
        {
            var page = new Page();

            var siteId = Guid.NewGuid();
            var pageId = Guid.NewGuid();
            var moduleId = Guid.NewGuid();

            var addPageModuleCommand = new AddPageModule
            {
                SiteId = siteId,
                PageId = pageId,
                ModuleId = moduleId,
                PageModuleId = Guid.NewGuid(),
                Title = "Title",
                Zone = "Zone"
            };
            var addPageModuleValidatorMock = new Mock<IValidator<AddPageModule>>();
            addPageModuleValidatorMock.Setup(x => x.Validate(addPageModuleCommand)).Returns(new ValidationResult());
            page.AddModule(addPageModuleCommand, addPageModuleValidatorMock.Object);

            var command = new UpdatePageModuleDetails
            {
                SiteId = siteId,
                PageId = pageId,
                ModuleId = moduleId,
                Title = "New Title",
                PageModuleLocalisations = new List<PageModuleLocalisation>
                {
                    new PageModuleLocalisation
                    {
                        LanguageId = Guid.NewGuid(),
                        Title = "Title"
                    },
                    new PageModuleLocalisation
                    {
                        LanguageId = Guid.NewGuid(),
                        Title = "Title"
                    }
                }
            };

            var repositoryMock = new Mock<IPageRepository>();
            repositoryMock.Setup(x => x.GetById(command.SiteId, command.PageId)).Returns(page);

            var validatorMock = new Mock<IValidator<UpdatePageModuleDetails>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            var createPageHandler = new UpdatePageModuleDetailsHandler(repositoryMock.Object, validatorMock.Object);
            createPageHandler.Handle(command);

            validatorMock.Verify(x => x.Validate(command));
            repositoryMock.Verify(x => x.Update(page));
        }
    }
}
