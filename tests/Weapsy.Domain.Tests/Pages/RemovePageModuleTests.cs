using System;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Pages;
using Weapsy.Domain.Pages.Commands;
using Weapsy.Domain.Pages.Events;

namespace Weapsy.Domain.Tests.Pages
{
    [TestFixture]
    public class RemovePageModuleTests
    {
        private Guid _moduleId;
        private Page _page;
        private RemovePageModule _command;
        private Mock<IValidator<RemovePageModule>> _validatorMock;
        private PageModuleRemoved _event;
        private PageModule _pageModule;

        [SetUp]
        public void Setup()
        {
            _moduleId = Guid.NewGuid();
            var addPageModuleCommand = new AddPageModule
            {
                SiteId = Guid.NewGuid(),
                PageId = Guid.NewGuid(),
                ModuleId = _moduleId,
                PageModuleId = Guid.NewGuid(),
                Title = "Title",
                Zone = "Zone"
            };
            var addPageModuleValidatorMock = new Mock<IValidator<AddPageModule>>();
            addPageModuleValidatorMock.Setup(x => x.Validate(addPageModuleCommand)).Returns(new ValidationResult());
            _page = new Page();
            _page.AddModule(addPageModuleCommand, addPageModuleValidatorMock.Object);
            _pageModule = _page.PageModules.FirstOrDefault(x => x.ModuleId == _moduleId);
            _command = new RemovePageModule
            {
                SiteId = addPageModuleCommand.SiteId,
                PageId = addPageModuleCommand.PageId,
                ModuleId = _moduleId,
            };
            _validatorMock = new Mock<IValidator<RemovePageModule>>();
            _validatorMock.Setup(x => x.Validate(_command)).Returns(new ValidationResult());
            _page.RemoveModule(_command, _validatorMock.Object);
            _event = _page.Events.OfType<PageModuleRemoved>().SingleOrDefault();
        }

        [Test]
        public void Should_call_validator()
        {
            _validatorMock.Verify(x => x.Validate(_command));
        }

        [Test]
        public void Should_remove_module()
        {
            Assert.AreEqual(PageModuleStatus.Deleted, _pageModule.Status);
        }

        [Test]
        public void Should_add_page_module_removed_event()
        {
            Assert.IsNotNull(_event);
        }

        [Test]
        public void Should_set_site_id_in_page_module_removed_event()
        {
            Assert.AreEqual(_page.SiteId, _event.SiteId);
        }

        [Test]
        public void Should_set_page_id_in_page_module_removed_event()
        {
            Assert.AreEqual(_page.Id, _event.AggregateRootId);
        }

        [Test]
        public void Should_set_module_id_in_page_module_removed_event()
        {
            Assert.AreEqual(_pageModule.ModuleId, _event.ModuleId);
        }

        [Test]
        public void Should_set_page_module_id_in_page_module_removed_event()
        {
            Assert.AreEqual(_pageModule.Id, _event.PageModuleId);
        }

        [Test]
        public void Should_throw_exception_if_module_does_not_exist()
        {
            _validatorMock = new Mock<IValidator<RemovePageModule>>();
            _validatorMock.Setup(x => x.Validate(_command)).Returns(new ValidationResult());
            Assert.Throws<Exception>(() => _page.RemoveModule(_command, _validatorMock.Object));
        }
    }
}
