using System;
using System.Collections.Generic;
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
    public class AddPageModuleTests
    {
        private AddPageModule _command;
        private Mock<IValidator<AddPageModule>> _validatorMock;
        private Page _page;
        private PageModuleAdded _event;
        private PageModule _pageModule;

        [SetUp]
        public void Setup()
        {
            _command = new AddPageModule
            {
                SiteId = Guid.NewGuid(),
                PageId = Guid.NewGuid(),
                ModuleId = Guid.NewGuid(),
                PageModuleId = Guid.NewGuid(),
                Title = "Title",
                Zone = "Zone",
                SortOrder = 1
            };
            _validatorMock = new Mock<IValidator<AddPageModule>>();
            _validatorMock.Setup(x => x.Validate(_command)).Returns(new ValidationResult());
            _page = new Page();
            _page.AddModule(_command, _validatorMock.Object);
            _event = _page.Events.OfType<PageModuleAdded>().SingleOrDefault();
            _pageModule = _page.PageModules.FirstOrDefault(x => x.Id == _command.PageModuleId);
        }

        [Test]
        public void Should_validate_command()
        {
            _validatorMock.Verify(x => x.Validate(_command));
        }

        [Test]
        public void Should_add_module()
        {
            Assert.IsNotNull(_pageModule);
        }

        [Test]
        public void Should_set_title()
        {
            Assert.AreEqual(_command.Title, _pageModule.Title);
        }

        [Test]
        public void Should_set_zone()
        {
            Assert.AreEqual(_command.Zone, _pageModule.Zone);
        }

        [Test]
        public void Should_set_sort_order()
        {
            Assert.AreEqual(_command.SortOrder, _pageModule.SortOrder);
        }

        [Test]
        public void Should_set_status_to_active()
        {
            Assert.AreEqual(PageModuleStatus.Active, _pageModule.Status);
        }

        [Test]
        public void Should_add_page_module_added_event()
        {
            Assert.IsNotNull(_event);
        }

        [Test]
        public void Should_set_site_id_in_page_module_added_event()
        {
            Assert.AreEqual(_page.SiteId, _event.SiteId);
        }

        [Test]
        public void Should_set_page_id_in_page_module_added_event()
        {
            Assert.AreEqual(_page.Id, _event.AggregateRootId);
        }

        [Test]
        public void Should_set_page_module_id_in_page_module_added_event()
        {
            Assert.AreEqual(_pageModule.Id, _event.PageModule.Id);
        }

        [Test]
        public void Should_set_title_in_page_module_added_event()
        {
            Assert.AreEqual(_pageModule.Title, _event.PageModule.Title);
        }

        [Test]
        public void Should_set_zone_in_page_module_added_event()
        {
            Assert.AreEqual(_pageModule.Zone, _event.PageModule.Zone);
        }

        [Test]
        public void Should_set_zsort_order_in_page_module_added_event()
        {
            Assert.AreEqual(_pageModule.SortOrder, _event.PageModule.SortOrder);
        }

        [Test]
        public void Should_set_status_in_page_module_added_event()
        {
            Assert.AreEqual(_pageModule.Status, _event.PageModule.Status);
        }

        [Test]
        public void Should_set_reordered_modules_in_page_module_added_event()
        {
            _command.PageModuleId = Guid.NewGuid();
            _page.AddModule(_command, _validatorMock.Object);
            _event = _page.Events.OfType<PageModuleAdded>().FirstOrDefault(x => x.PageModule.Id == _command.PageModuleId);
            Assert.NotNull(_event.ReorderedModules.FirstOrDefault(x => x.PageModuleId == _pageModule.Id));
        }

        [Test]
        public void Should_throw_exception_if_module_is_already_added()
        {
            Assert.Throws<Exception>(() => _page.AddModule(_command, _validatorMock.Object));
        }
    }
}
