using System;
using System.Collections.Generic;
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
    public class AddVersionTests
    {
        private AddVersion _command;
        private Mock<IValidator<AddVersion>> _validatorMock;
        private TextModule _textModule;
        private TextVersion _newVersion;
        private VersionAdded _event;

        [SetUp]
        public void Setup()
        {
            _textModule = TextModuleFactory.Get();

            _command = new AddVersion
            {
                SiteId = Guid.NewGuid(),
                ModuleId = _textModule.ModuleId,
                Id = _textModule.Id,
                VersionId = Guid.NewGuid(),
                Content = "Content",
                Description = "Description",
                Status = TextVersionStatus.Published,
                VersionLocalisations = new List<AddVersion.VersionLocalisation>()
                {
                    new AddVersion.VersionLocalisation
                    {
                        LanguageId = Guid.NewGuid(),
                        Content = "Localised content"
                    }
                }
            };

            _validatorMock = new Mock<IValidator<AddVersion>>();
            _validatorMock.Setup(x => x.Validate(_command)).Returns(new ValidationResult());

            _textModule.AddVersion(_command, _validatorMock.Object);

            _newVersion = _textModule.TextVersions.FirstOrDefault(x => x.Id == _command.VersionId);

            _event = _textModule.Events.OfType<VersionAdded>().SingleOrDefault();
        }

        [Test]
        public void Should_validate_command()
        {
            _validatorMock.Verify(x => x.Validate(_command));
        }

        [Test]
        public void Should_add_new_version()
        {
            Assert.NotNull(_newVersion);
        }

        [Test]
        public void Should_set_id_in_new_version()
        {
            Assert.AreEqual(_command.VersionId, _newVersion.Id);
        }

        [Test]
        public void Should_set_text_module_id_in_new_version()
        {
            Assert.AreEqual(_command.Id, _newVersion.TextModuleId);
        }

        [Test]
        public void Should_set_content_in_new_version()
        {
            Assert.AreEqual(_command.Content, _newVersion.Content);
        }

        [Test]
        public void Should_set_description_in_new_version()
        {
            Assert.AreEqual(_command.Description, _newVersion.Description);
        }

        [Test]
        public void Should_set_status_in_new_version()
        {
            Assert.AreEqual(_command.Status, _newVersion.Status);
        }

        [Test]
        public void Should_set_localisation_in_new_version()
        {
            Assert.NotNull(_newVersion.TextLocalisations[0]);
        }

        [Test]
        public void Should_set_language_id_in_new_version_localisation()
        {
            Assert.AreEqual(_command.VersionLocalisations[0].LanguageId, _newVersion.TextLocalisations[0].LanguageId);
        }

        [Test]
        public void Should_set_content_in_new_version_localisation()
        {
            Assert.AreEqual(_command.VersionLocalisations[0].Content, _newVersion.TextLocalisations[0].Content);
        }

        [Test]
        public void Should_add_version_added_event()
        {
            Assert.IsNotNull(_event);
        }

        [Test]
        public void Should_set_id_in_version_added_event()
        {
            Assert.AreEqual(_textModule.Id, _event.AggregateRootId);
        }

        [Test]
        public void Should_set_site_id_in_version_added_event()
        {
            Assert.AreEqual(_command.SiteId, _event.SiteId);
        }

        [Test]
        public void Should_set_module_id_in_version_added_event()
        {
            Assert.AreEqual(_textModule.ModuleId, _event.ModuleId);
        }

        [Test]
        public void Should_set_version_id_in_version_added_event()
        {
            Assert.AreEqual(_newVersion.Id, _event.VersionId);
        }

        [Test]
        public void Should_set_content_in_version_added_event()
        {
            Assert.AreEqual(_newVersion.Content, _event.Content);
        }

        [Test]
        public void Should_set_description_in_version_added_event()
        {
            Assert.AreEqual(_newVersion.Description, _event.Description);
        }

        [Test]
        public void Should_set_version_status_in_version_added_event()
        {
            Assert.AreEqual(_newVersion.Status, _event.Status);
        }
    }
}
