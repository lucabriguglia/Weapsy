using System;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using NUnit.Framework;
using Weapsy.Apps.Text.Domain;
using Weapsy.Apps.Text.Domain.Commands;

namespace Weapsy.Apps.Text.Tests.Domain
{
    [TestFixture]
    public class AddVersionStatusTests
    {
        [Test]
        [TestCase(TextVersionStatus.DelayedPublish)]
        [TestCase(TextVersionStatus.Deleted)]
        [TestCase(TextVersionStatus.PreviouslyPublished)]
        [TestCase(TextVersionStatus.Rejected)]
        public void Should_throw_an_exception_if_status_is_not_published_or_draft(TextVersionStatus status)
        {
            var textModule = TextModuleFactory.Get();

            var command = new AddVersion
            {
                Status = status,
            };

            var validatorMock = new Mock<IValidator<AddVersion>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            Assert.Throws<Exception>(() => textModule.AddVersion(command, validatorMock.Object));
        }

        [Test]
        public void Should_change_status_of_current_published_version_to_previously_published()
        {
            var textModule = TextModuleFactory.Get();

            var currentPublishedVersion = textModule.TextVersions.Single(x => x.Status == TextVersionStatus.Published);

            var command = new AddVersion
            {
                Status = TextVersionStatus.Published,
            };

            var validatorMock = new Mock<IValidator<AddVersion>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            textModule.AddVersion(command, validatorMock.Object);

            Assert.AreEqual(TextVersionStatus.PreviouslyPublished, currentPublishedVersion.Status);
        }

        [Test]
        public void Should_not_change_status_of_current_published_version_if_new_version_status_is_draft()
        {
            var textModule = TextModuleFactory.Get();

            var currentPublishedVersion = textModule.TextVersions.Single(x => x.Status == TextVersionStatus.Published);

            var command = new AddVersion
            {
                Status = TextVersionStatus.Draft,
            };

            var validatorMock = new Mock<IValidator<AddVersion>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            textModule.AddVersion(command, validatorMock.Object);

            Assert.AreEqual(TextVersionStatus.Published, currentPublishedVersion.Status);
        }
    }
}
