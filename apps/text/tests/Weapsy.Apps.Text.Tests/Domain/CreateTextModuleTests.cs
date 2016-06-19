using System;
using System.Linq;
using NUnit.Framework;
using Moq;
using FluentValidation;
using FluentValidation.Results;
using Weapsy.Apps.Text.Domain.Commands;
using Weapsy.Apps.Text.Domain;
using Weapsy.Apps.Text.Domain.Events;

namespace Weapsy.Apps.Text.Domain.Tests
{
    [TestFixture]
    public class CreateTextModuleTests
    {
        private CreateTextModule command;
        private Mock<IValidator<CreateTextModule>> validatorMock;
        private TextModule textModule;
        private TextModuleCreated @event;

        [SetUp]
        public void Setup()
        {
            command = new CreateTextModule
            {
                SiteId = Guid.NewGuid(),
                ModuleId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Content = "Content"
            };
            validatorMock = new Mock<IValidator<CreateTextModule>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());
            textModule = TextModule.CreateNew(command, validatorMock.Object);
            @event = textModule.Events.OfType<TextModuleCreated>().SingleOrDefault();
        }

        [Test]
        public void Should_validate_command()
        {
            validatorMock.Verify(x => x.Validate(command));
        }

        [Test]
        public void Should_set_module_id()
        {
            Assert.AreEqual(command.ModuleId, textModule.ModuleId);
        }

        [Test]
        public void Should_set_id()
        {
            Assert.AreEqual(command.Id, textModule.Id);
        }

        [Test]
        public void Should_set_status_to_active()
        {
            Assert.AreEqual(TextModuleStatus.Active, textModule.Status);
        }

        [Test]
        public void Should_set_version_content()
        {
            Assert.AreEqual(command.Content, textModule.TextVersions[0].Content);
        }

        [Test]
        public void Should_set_version_status_to_published()
        {
            Assert.AreEqual(TextVersionStatus.Published, textModule.TextVersions[0].Status);
        }

        [Test]
        public void Should_add_text_module_created_event()
        {
            Assert.IsNotNull(@event);
        }

        [Test]
        public void Should_set_id_in_text_module_created_event()
        {
            Assert.AreEqual(textModule.Id, @event.AggregateRootId);
        }

        [Test]
        public void Should_set_site_id_in_text_module_created_event()
        {
            Assert.AreEqual(command.SiteId, @event.SiteId);
        }

        [Test]
        public void Should_set_module_id_in_text_module_created_event()
        {
            Assert.AreEqual(textModule.ModuleId, @event.ModuleId);
        }

        [Test]
        public void Should_set_version_id_in_text_module_created_event()
        {
            Assert.AreEqual(textModule.TextVersions[0].Id, @event.VersionId);
        }

        [Test]
        public void Should_set_content_in_text_module_created_event()
        {
            Assert.AreEqual(textModule.TextVersions[0].Content, @event.Content);
        }
    }
}
