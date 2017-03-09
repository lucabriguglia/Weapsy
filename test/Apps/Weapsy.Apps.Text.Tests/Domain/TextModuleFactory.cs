using FluentValidation;
using FluentValidation.Results;
using Moq;
using System;
using System.Collections.Generic;
using Weapsy.Apps.Text.Domain;
using Weapsy.Apps.Text.Domain.Commands;

namespace Weapsy.Apps.Text.Tests.Domain
{
    public static class TextModuleFactory
    {
        private static Mock<IValidator<AddVersion>> _addVersionValidatorMock;

        public static TextModule Get()
        {
            var siteId = Guid.NewGuid();

            var createCommand = new CreateTextModule
            {
                SiteId = siteId,
                ModuleId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Content = "Content"
            };

            var createValidatorMock = new Mock<IValidator<CreateTextModule>>();
            createValidatorMock.Setup(x => x.Validate(createCommand)).Returns(new ValidationResult());

            var textModule = TextModule.CreateNew(createCommand, createValidatorMock.Object);

            var addVersionCommand = new AddVersion
            {
                SiteId = siteId,
                ModuleId = textModule.ModuleId,
                Id = textModule.Id,
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

            _addVersionValidatorMock = new Mock<IValidator<AddVersion>>();
            _addVersionValidatorMock.Setup(x => x.Validate(addVersionCommand)).Returns(new ValidationResult());

            textModule.AddVersion(addVersionCommand, _addVersionValidatorMock.Object);

            textModule.Events.Clear();

            return textModule;
        }
    }
}
