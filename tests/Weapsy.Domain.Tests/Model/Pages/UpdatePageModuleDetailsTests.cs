using System;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Model.Pages;
using Weapsy.Domain.Model.Pages.Commands;
using Weapsy.Domain.Model.Pages.Events;
using System.Collections.Generic;

namespace Weapsy.Domain.Tests.Pages
{
    [TestFixture]
    public class UpdatePageModuleDetailsTests
    {
        private UpdatePageModuleDetails command;
        private Mock<IValidator<UpdatePageModuleDetails>> validatorMock;
        private Page page;
        private PageModule pageModule;
        private PageModuleDetailsUpdated @event;

        [SetUp]
        public void Setup()
        {
            page = new Page();

            var siteId = Guid.NewGuid();
            var pageId = Guid.NewGuid();
            var moduleId = Guid.NewGuid();

            var addPageModuleCommand = new AddPageModule
            {
                SiteId = siteId,
                PageId = pageId,
                ModuleId = moduleId,
                Id = Guid.NewGuid(),
                Title = "Title",
                Zone = "Zone"
            };
            var addPageModuleValidatorMock = new Mock<IValidator<AddPageModule>>();
            addPageModuleValidatorMock.Setup(x => x.Validate(addPageModuleCommand)).Returns(new ValidationResult());
            page.AddModule(addPageModuleCommand, addPageModuleValidatorMock.Object);

            command = new UpdatePageModuleDetails
            {
                SiteId = siteId,
                PageId = pageId,
                ModuleId = moduleId,
                Title = "New Title",
                PageModuleLocalisations = new List<UpdatePageModuleDetails.PageModuleLocalisation>
                {
                    new UpdatePageModuleDetails.PageModuleLocalisation
                    {
                        LanguageId = Guid.NewGuid(),
                        Title = "Title"
                    }
                }
            };
            validatorMock = new Mock<IValidator<UpdatePageModuleDetails>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());            
            page.UpdateModule(command, validatorMock.Object);
            pageModule = page.PageModules.FirstOrDefault(x => x.ModuleId == moduleId);
            @event = page.Events.OfType<PageModuleDetailsUpdated>().SingleOrDefault();
        }

        [Test]
        public void Should_validate_command()
        {
            validatorMock.Verify(x => x.Validate(command));
        }

        [Test]
        public void Should_set_title()
        {
            Assert.AreEqual(command.Title, pageModule.Title);
        }

        [Test]
        public void Should_set_localisation_language_id()
        {
            Assert.AreEqual(command.PageModuleLocalisations[0].LanguageId, pageModule.PageModuleLocalisations.FirstOrDefault().LanguageId);
        }

        [Test]
        public void Should_set_localisation_title()
        {
            Assert.AreEqual(command.PageModuleLocalisations[0].Title, pageModule.PageModuleLocalisations.FirstOrDefault().Title);
        }

        [Test]
        public void Should_add_page_module_details_updated_event()
        {
            Assert.IsNotNull(@event);
        }

        [Test]
        public void Should_set_site_id_in_page_module_details_updated_event()
        {
            Assert.AreEqual(page.SiteId, @event.SiteId);
        }

        [Test]
        public void Should_set_id_in_page_module_details_updated_event()
        {
            Assert.AreEqual(page.Id, @event.AggregateRootId);
        }

        [Test]
        public void Should_set_page_module_in_page_module_details_updated_event()
        {
            Assert.AreEqual(pageModule, @event.PageModule);
        }

        [Test]
        public void Should_throw_exception_if_language_is_already_added_to_page_module_localisations()
        {
            var languageId = Guid.NewGuid();
            command.PageModuleLocalisations.Add(new UpdatePageModuleDetails.PageModuleLocalisation
            {
                LanguageId = languageId
            });
            command.PageModuleLocalisations.Add(new UpdatePageModuleDetails.PageModuleLocalisation
            {
                LanguageId = languageId
            });
            Assert.Throws<Exception>(() => page.UpdateModule(command, validatorMock.Object));
        }
    }
}