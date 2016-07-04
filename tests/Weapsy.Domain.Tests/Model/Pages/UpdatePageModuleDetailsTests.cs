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
        private UpdatePageModuleDetails _command;
        private Mock<IValidator<UpdatePageModuleDetails>> _validatorMock;
        private Page _page;
        private PageModule _pageModule;
        private PageModuleDetailsUpdated _event;

        [SetUp]
        public void Setup()
        {
            _page = new Page();

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
            _page.AddModule(addPageModuleCommand, addPageModuleValidatorMock.Object);

            _command = new UpdatePageModuleDetails
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
            _validatorMock = new Mock<IValidator<UpdatePageModuleDetails>>();
            _validatorMock.Setup(x => x.Validate(_command)).Returns(new ValidationResult());            
            _page.UpdateModule(_command, _validatorMock.Object);
            _pageModule = _page.PageModules.FirstOrDefault(x => x.ModuleId == moduleId);
            _event = _page.Events.OfType<PageModuleDetailsUpdated>().SingleOrDefault();
        }

        [Test]
        public void Should_validate_command()
        {
            _validatorMock.Verify(x => x.Validate(_command));
        }

        [Test]
        public void Should_set_title()
        {
            Assert.AreEqual(_command.Title, _pageModule.Title);
        }

        [Test]
        public void Should_set_localisation_language_id()
        {
            Assert.AreEqual(_command.PageModuleLocalisations[0].LanguageId, _pageModule.PageModuleLocalisations.FirstOrDefault().LanguageId);
        }

        [Test]
        public void Should_set_localisation_title()
        {
            Assert.AreEqual(_command.PageModuleLocalisations[0].Title, _pageModule.PageModuleLocalisations.FirstOrDefault().Title);
        }

        [Test]
        public void Should_add_page_module_details_updated_event()
        {
            Assert.IsNotNull(_event);
        }

        [Test]
        public void Should_set_site_id_in_page_module_details_updated_event()
        {
            Assert.AreEqual(_page.SiteId, _event.SiteId);
        }

        [Test]
        public void Should_set_id_in_page_module_details_updated_event()
        {
            Assert.AreEqual(_page.Id, _event.AggregateRootId);
        }

        [Test]
        public void Should_set_page_module_in_page_module_details_updated_event()
        {
            Assert.AreEqual(_pageModule, _event.PageModule);
        }

        [Test]
        public void Should_throw_exception_if_language_is_already_added_to_page_module_localisations()
        {
            var languageId = Guid.NewGuid();
            _command.PageModuleLocalisations.Add(new UpdatePageModuleDetails.PageModuleLocalisation
            {
                LanguageId = languageId
            });
            _command.PageModuleLocalisations.Add(new UpdatePageModuleDetails.PageModuleLocalisation
            {
                LanguageId = languageId
            });
            Assert.Throws<Exception>(() => _page.UpdateModule(_command, _validatorMock.Object));
        }
    }
}