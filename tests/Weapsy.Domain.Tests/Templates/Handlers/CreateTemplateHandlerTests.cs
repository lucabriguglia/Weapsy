using System;
using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Templates;
using Weapsy.Domain.Templates.Commands;
using Weapsy.Domain.Templates.Handlers;

namespace Weapsy.Domain.Tests.Templates.Handlers
{
    [TestFixture]
    public class CreateTemplateHandlerTests
    {
        [Test]
        public void Should_throw_validation_exception_when_validation_fails()
        {
            var command = new CreateTemplate
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Description = "Description",
                ViewName = "ViewName"
            };

            var templateRepositoryMock = new Mock<ITemplateRepository>();

            var validatorMock = new Mock<IValidator<CreateTemplate>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Id", "Id Error") }));

            var createTemplateHandler = new CreateTemplateHandler(templateRepositoryMock.Object, validatorMock.Object);

            Assert.Throws<Exception>(() => createTemplateHandler.Handle(command));
        }

        [Test]
        public void Should_validate_command_and_save_new_template()
        {
            var command = new CreateTemplate
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Description = "Description",
                ViewName = "ViewName"
            };

            var templateRepositoryMock = new Mock<ITemplateRepository>();
            templateRepositoryMock.Setup(x => x.Create(It.IsAny<Template>()));

            var validatorMock = new Mock<IValidator<CreateTemplate>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            var createTemplateHandler = new CreateTemplateHandler(templateRepositoryMock.Object, validatorMock.Object);
            createTemplateHandler.Handle(command);

            validatorMock.Verify(x => x.Validate(command));
            templateRepositoryMock.Verify(x => x.Create(It.IsAny<Template>()));
        }
    }
}
