using System;
using System.Linq;
using NUnit.Framework;
using Moq;
using FluentValidation;
using FluentValidation.Results;
using Weapsy.Apps.Text.Domain.Commands;
using Weapsy.Apps.Text.Domain.Events;
using Weapsy.Apps.Text.Tests.Domain;
using System.Collections.Generic;

namespace Weapsy.Apps.Text.Domain.Tests
{
    [TestFixture]
    public class AddVersionTests
    {
        private AddVersion command;
        private Mock<IValidator<AddVersion>> validatorMock;
        private TextModule textModule;
        private TextVersion newVersion;
        private VersionAdded @event;

        [SetUp]
        public void Setup()
        {
            textModule = TextModuleFactory.Get();

            command = new AddVersion
            {
                SiteId = Guid.NewGuid(),
                ModuleId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
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

            validatorMock = new Mock<IValidator<AddVersion>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            textModule.AddVersion(command, validatorMock.Object);

            newVersion = textModule.TextVersions.FirstOrDefault(x => x.Id == command.VersionId);

            @event = textModule.Events.OfType<VersionAdded>().SingleOrDefault();
        }

        [Test]
        public void Should_validate_command()
        {
            validatorMock.Verify(x => x.Validate(command));
        }

        [Test]
        public void Should_add_new_version()
        {
            Assert.NotNull(newVersion);
        }

        [Test]
        public void Should_set_id_in_new_version()
        {
            Assert.AreEqual(command.VersionId, newVersion.Id);
        }

        [Test]
        public void Should_set_status_text_module_in_new_version()
        {
            Assert.AreEqual(command.Id, newVersion.TextModuleId);
        }

        [Test]
        public void Should_set_content_in_new_version()
        {
            Assert.AreEqual(command.Content, newVersion.Content);
        }

        [Test]
        public void Should_set_description_in_new_version()
        {
            Assert.AreEqual(command.Description, newVersion.Description);
        }

        [Test]
        public void Should_set_status_in_new_version()
        {
            Assert.AreEqual(command.Status, newVersion.Status);
        }

        [Test]
        public void Should_set_localisation_in_new_version()
        {
            Assert.NotNull(newVersion.TextLocalisations[0]);
        }

        [Test]
        public void Should_set_language_id_in_new_version_localisation()
        {
            Assert.AreEqual(command.VersionLocalisations[0].LanguageId, newVersion.TextLocalisations[0].LanguageId);
        }

        [Test]
        public void Should_set_content_in_new_version_localisation()
        {
            Assert.AreEqual(command.VersionLocalisations[0].Content, newVersion.TextLocalisations[0].Content);
        }

        [Test]
        public void Should_add_version_added_event()
        {
            Assert.IsNotNull(@event);
        }

        [Test]
        public void Should_set_id_in_version_added_event()
        {
            Assert.AreEqual(textModule.Id, @event.AggregateRootId);
        }

        [Test]
        public void Should_set_site_id_in_version_added_event()
        {
            Assert.AreEqual(command.SiteId, @event.SiteId);
        }

        [Test]
        public void Should_set_module_id_in_version_added_event()
        {
            Assert.AreEqual(textModule.ModuleId, @event.ModuleId);
        }

        [Test]
        public void Should_set_version_id_in_version_added_event()
        {
            Assert.AreEqual(newVersion.Id, @event.VersionId);
        }

        [Test]
        public void Should_set_content_in_version_added_event()
        {
            Assert.AreEqual(newVersion.Content, @event.Content);
        }

        [Test]
        public void Should_set_description_in_version_added_event()
        {
            Assert.AreEqual(newVersion.Description, @event.Description);
        }

        [Test]
        public void Should_set_version_status_in_version_added_event()
        {
            Assert.AreEqual(newVersion.Status, @event.Status);
        }
    }
}
