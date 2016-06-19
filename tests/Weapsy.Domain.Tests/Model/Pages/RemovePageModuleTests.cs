using System;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Model.Pages;
using Weapsy.Domain.Model.Pages.Commands;
using Weapsy.Domain.Model.Pages.Events;

namespace Weapsy.Domain.Tests.Pages
{
    [TestFixture]
    public class RemovePageModuleTests
    {
        private Guid moduleId;
        private Page page;
        private RemovePageModule command;
        private Mock<IValidator<RemovePageModule>> validatorMock;
        private PageModuleRemoved @event;
        private PageModule pageModule;

        [SetUp]
        public void Setup()
        {
            moduleId = Guid.NewGuid();
            var addPageModuleCommand = new AddPageModule
            {
                SiteId = Guid.NewGuid(),
                PageId = Guid.NewGuid(),
                ModuleId = moduleId,
                Id = Guid.NewGuid(),
                Title = "Title",
                Zone = "Zone"
            };
            var addPageModuleValidatorMock = new Mock<IValidator<AddPageModule>>();
            addPageModuleValidatorMock.Setup(x => x.Validate(addPageModuleCommand)).Returns(new ValidationResult());
            page = new Page();
            page.AddModule(addPageModuleCommand, addPageModuleValidatorMock.Object);
            pageModule = page.PageModules.FirstOrDefault(x => x.ModuleId == moduleId);
            command = new RemovePageModule
            {
                SiteId = addPageModuleCommand.SiteId,
                PageId = addPageModuleCommand.PageId,
                ModuleId = moduleId,
            };
            validatorMock = new Mock<IValidator<RemovePageModule>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());
            page.RemoveModule(command, validatorMock.Object);
            @event = page.Events.OfType<PageModuleRemoved>().SingleOrDefault();
        }

        [Test]
        public void Should_call_validator()
        {
            validatorMock.Verify(x => x.Validate(command));
        }

        [Test]
        public void Should_remove_module()
        {
            Assert.AreEqual(PageModuleStatus.Deleted, pageModule.Status);
        }

        [Test]
        public void Should_add_page_module_removed_event()
        {
            Assert.IsNotNull(@event);
        }

        [Test]
        public void Should_set_site_id_in_page_module_removed_event()
        {
            Assert.AreEqual(page.SiteId, @event.SiteId);
        }

        [Test]
        public void Should_set_page_id_in_page_module_removed_event()
        {
            Assert.AreEqual(page.Id, @event.AggregateRootId);
        }

        [Test]
        public void Should_set_module_id_in_page_module_removed_event()
        {
            Assert.AreEqual(pageModule.ModuleId, @event.ModuleId);
        }

        [Test]
        public void Should_set_page_module_id_in_page_module_removed_event()
        {
            Assert.AreEqual(pageModule.Id, @event.PageModuleId);
        }

        [Test]
        public void Should_throw_exception_if_module_does_not_exist()
        {
            validatorMock = new Mock<IValidator<RemovePageModule>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());
            Assert.Throws<Exception>(() => page.RemoveModule(command, validatorMock.Object));
        }
    }
}
