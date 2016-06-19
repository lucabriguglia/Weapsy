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
    public class AddPageModuleTests
    {
        private AddPageModule command;
        private Mock<IValidator<AddPageModule>> validatorMock;
        private Page page;
        private PageModuleAdded @event;
        private PageModule pageModule;

        [SetUp]
        public void Setup()
        {
            command = new AddPageModule
            {
                SiteId = Guid.NewGuid(),
                PageId = Guid.NewGuid(),
                ModuleId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Title = "Title",
                Zone = "Zone",
                SortOrder = 1
            };
            validatorMock = new Mock<IValidator<AddPageModule>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());
            page = new Page();
            page.AddModule(command, validatorMock.Object);
            @event = page.Events.OfType<PageModuleAdded>().SingleOrDefault();
            pageModule = page.PageModules.FirstOrDefault(x => x.Id == command.Id);
        }

        [Test]
        public void Should_validate_command()
        {
            validatorMock.Verify(x => x.Validate(command));
        }

        [Test]
        public void Should_add_module()
        {
            Assert.IsNotNull(pageModule);
        }

        [Test]
        public void Should_set_title()
        {
            Assert.AreEqual(command.Title, pageModule.Title);
        }

        [Test]
        public void Should_set_zone()
        {
            Assert.AreEqual(command.Zone, pageModule.Zone);
        }

        [Test]
        public void Should_set_sort_order()
        {
            Assert.AreEqual(command.SortOrder, pageModule.SortOrder);
        }

        [Test]
        public void Should_set_status_to_active()
        {
            Assert.AreEqual(PageModuleStatus.Active, pageModule.Status);
        }

        [Test]
        public void Should_add_page_module_added_event()
        {
            Assert.IsNotNull(@event);
        }

        [Test]
        public void Should_set_site_id_in_page_module_added_event()
        {
            Assert.AreEqual(page.SiteId, @event.SiteId);
        }

        [Test]
        public void Should_set_page_id_in_page_module_added_event()
        {
            Assert.AreEqual(page.Id, @event.AggregateRootId);
        }

        [Test]
        public void Should_set_module_id_in_page_module_added_event()
        {
            Assert.AreEqual(pageModule.ModuleId, @event.ModuleId);
        }

        [Test]
        public void Should_set_page_module_id_in_page_module_added_event()
        {
            Assert.AreEqual(pageModule.Id, @event.AggregateRootId);
        }

        [Test]
        public void Should_set_title_in_page_module_added_event()
        {
            Assert.AreEqual(pageModule.Title, @event.Title);
        }

        [Test]
        public void Should_set_zone_in_page_module_added_event()
        {
            Assert.AreEqual(pageModule.Zone, @event.Zone);
        }

        [Test]
        public void Should_set_zsort_order_in_page_module_added_event()
        {
            Assert.AreEqual(pageModule.SortOrder, @event.SortOrder);
        }

        [Test]
        public void Should_set_status_in_page_module_added_event()
        {
            Assert.AreEqual(pageModule.Status, @event.PageModuleStatus);
        }

        [Test]
        public void Should_set_reordered_modules_in_page_module_added_event()
        {
            command.ModuleId = Guid.NewGuid();
            page.AddModule(command, validatorMock.Object);
            @event = page.Events.OfType<PageModuleAdded>().FirstOrDefault(x => x.ModuleId == command.ModuleId);
            Assert.NotNull(@event.ReorderedModules.FirstOrDefault(x => x.ModuleId == pageModule.ModuleId));
        }

        [Test]
        public void Should_throw_exception_if_module_is_already_added()
        {
            Assert.Throws<Exception>(() => page.AddModule(command, validatorMock.Object));
        }
    }
}
