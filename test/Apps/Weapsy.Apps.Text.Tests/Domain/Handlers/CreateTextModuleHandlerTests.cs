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
    public class CreateTextModuleHandlerTests
    {
        [Test]
        public void Should_throw_validation_exception_when_validation_fails()
        {
            var command = new CreateTextModule
            {
                SiteId = Guid.NewGuid(),
                ModuleId = Guid.NewGuid(),
                Id = Guid.Empty,
                Content = "Content"
            };

            var textModuleRepositoryMock = new Mock<ITextModuleRepository>();

            var validatorMock = new Mock<IValidator<CreateTextModule>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Id", "Id Error") }));

            var createTextModuleHandler = new CreateTextModuleHandler(textModuleRepositoryMock.Object, validatorMock.Object);

            Assert.Throws<Exception>(() => createTextModuleHandler.Handle(command));
        }

        [Test]
        public void Should_validate_command_and_save_new_textModule()
        {
            var command = new CreateTextModule
            {
                SiteId = Guid.NewGuid(),
                ModuleId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Content = "Content"
            };

            var textModuleRepositoryMock = new Mock<ITextModuleRepository>();
            textModuleRepositoryMock.Setup(x => x.Create(It.IsAny<TextModule>()));

            var validatorMock = new Mock<IValidator<CreateTextModule>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            var createTextModuleHandler = new CreateTextModuleHandler(textModuleRepositoryMock.Object, validatorMock.Object);
            createTextModuleHandler.Handle(command);

            validatorMock.Verify(x => x.Validate(command));
            textModuleRepositoryMock.Verify(x => x.Create(It.IsAny<TextModule>()));
        }
    }
}
