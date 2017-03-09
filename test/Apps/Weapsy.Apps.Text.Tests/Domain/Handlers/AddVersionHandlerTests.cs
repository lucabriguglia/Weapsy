using System;
using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using NUnit.Framework;
using Weapsy.Apps.Text.Domain;
using Weapsy.Apps.Text.Domain.Commands;
using Weapsy.Apps.Text.Domain.Handlers;

namespace Weapsy.Apps.Text.Tests.Domain.Handlers
{
    [TestFixture]
    public class AddVersionHandlerTests
    {
        [Test]
        public void Should_throw_validation_exception_when_validation_fails()
        {
            var command = new AddVersion
            {
                SiteId = Guid.NewGuid(),
                ModuleId = Guid.NewGuid(),
                Id = Guid.Empty,
                Content = "Content"
            };

            var textModuleRepositoryMock = new Mock<ITextModuleRepository>();

            var validatorMock = new Mock<IValidator<AddVersion>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Id", "Id Error") }));

            var addVersionHandler = new AddVersionHandler(textModuleRepositoryMock.Object, validatorMock.Object);

            Assert.Throws<Exception>(() => addVersionHandler.Handle(command));
        }

        [Test]
        public void Should_validate_command_and_save_new_textModule()
        {
            var textModule = TextModuleFactory.Get();

            var command = new AddVersion
            {
                SiteId = Guid.NewGuid(),
                ModuleId = textModule.ModuleId,
                Id = Guid.NewGuid(),
                Content = "Content",
                Status = TextVersionStatus.Draft
            };

            var textModuleRepositoryMock = new Mock<ITextModuleRepository>();
            textModuleRepositoryMock.Setup(x => x.GetByModuleId(command.ModuleId)).Returns(textModule);
            textModuleRepositoryMock.Setup(x => x.Update(It.IsAny<TextModule>()));

            var validatorMock = new Mock<IValidator<AddVersion>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            var addVersionHandler = new AddVersionHandler(textModuleRepositoryMock.Object, validatorMock.Object);
            addVersionHandler.Handle(command);

            validatorMock.Verify(x => x.Validate(command));
            textModuleRepositoryMock.Verify(x => x.Update(It.IsAny<TextModule>()));
        }
    }
}
