using System;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using NUnit.Framework;
using Weapsy.Apps.Text.Domain;
using Weapsy.Apps.Text.Domain.Commands;
using Weapsy.Apps.Text.Domain.Events;

namespace Weapsy.Apps.Text.Tests.Domain
{
    [TestFixture]
    public class CreateTextModuleTests
    {
        private CreateTextModule _command;
        private Mock<IValidator<CreateTextModule>> _validatorMock;
        private TextModule _textModule;
        private TextModuleCreated _event;

        [SetUp]
        public void Setup()
        {
            _command = new CreateTextModule
            {
                SiteId = Guid.NewGuid(),
                ModuleId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Content = "Content"
            };
            _validatorMock = new Mock<IValidator<CreateTextModule>>();
            _validatorMock.Setup(x => x.Validate(_command)).Returns(new ValidationResult());
            _textModule = TextModule.CreateNew(_command, _validatorMock.Object);
            _event = _textModule.Events.OfType<TextModuleCreated>().SingleOrDefault();
        }

        [Test]
        public void Should_validate_command()
        {
            _validatorMock.Verify(x => x.Validate(_command));
        }

        [Test]
        public void Should_set_module_id()
        {
            Assert.AreEqual(_command.ModuleId, _textModule.ModuleId);
        }

        [Test]
        public void Should_set_id()
        {
            Assert.AreEqual(_command.Id, _textModule.Id);
        }

        [Test]
        public void Should_set_status_to_active()
        {
            Assert.AreEqual(TextModuleStatus.Active, _textModule.Status);
        }

        [Test]
        public void Should_set_version_content()
        {
            Assert.AreEqual(_command.Content, _textModule.TextVersions[0].Content);
        }

        [Test]
        public void Should_set_version_status_to_published()
        {
            Assert.AreEqual(TextVersionStatus.Published, _textModule.TextVersions[0].Status);
        }

        [Test]
        public void Should_add_text_module_created_event()
        {
            Assert.IsNotNull(_event);
        }

        [Test]
        public void Should_set_id_in_text_module_created_event()
        {
            Assert.AreEqual(_textModule.Id, _event.AggregateRootId);
        }

        [Test]
        public void Should_set_site_id_in_text_module_created_event()
        {
            Assert.AreEqual(_command.SiteId, _event.SiteId);
        }

        [Test]
        public void Should_set_module_id_in_text_module_created_event()
        {
            Assert.AreEqual(_textModule.ModuleId, _event.ModuleId);
        }

        [Test]
        public void Should_set_version_id_in_text_module_created_event()
        {
            Assert.AreEqual(_textModule.TextVersions[0].Id, _event.VersionId);
        }

        [Test]
        public void Should_set_content_in_text_module_created_event()
        {
            Assert.AreEqual(_textModule.TextVersions[0].Content, _event.Content);
        }
    }
}
