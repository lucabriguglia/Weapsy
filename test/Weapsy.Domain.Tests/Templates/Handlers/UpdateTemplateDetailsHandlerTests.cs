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
    public class UpdateTemplateDetailsHandlerTests
    {
        [Test]
        public void Should_throw_exception_when_template_is_not_found()
        {
            var command = new UpdateTemplateDetails
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Description = "Description",
                ViewName = "ViewName"
            };

            var templateRepositoryMock = new Mock<ITemplateRepository>();
            templateRepositoryMock.Setup(x => x.GetById(command.Id)).Returns((Template)null);

            var validatorMock = new Mock<IValidator<UpdateTemplateDetails>>();

            var createTemplateHandler = new UpdateTemplateDetailsHandler(templateRepositoryMock.Object, validatorMock.Object);

            Assert.Throws<Exception>(() => createTemplateHandler.Handle(command));
        }

        [Test]
        public void Should_throw_validation_exception_when_validation_fails()
        {
            var command = new UpdateTemplateDetails
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Description = "Description",
                ViewName = "ViewName"
            };

            var templateRepositoryMock = new Mock<ITemplateRepository>();
            templateRepositoryMock.Setup(x => x.GetById(command.Id)).Returns(new Template());

            var validatorMock = new Mock<IValidator<UpdateTemplateDetails>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Name", "Name Error") }));

            var createTemplateHandler = new UpdateTemplateDetailsHandler(templateRepositoryMock.Object, validatorMock.Object);

            Assert.Throws<Exception>(() => createTemplateHandler.Handle(command));
        }

        [Test]
        public void Should_validate_command_and_save_new_template()
        {
            var command = new UpdateTemplateDetails
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Description = "Description",
                ViewName = "ViewName"
            };

            var templateRepositoryMock = new Mock<ITemplateRepository>();
            templateRepositoryMock.Setup(x => x.GetById(command.Id)).Returns(new Template());
            templateRepositoryMock.Setup(x => x.Update(It.IsAny<Template>()));

            var validatorMock = new Mock<IValidator<UpdateTemplateDetails>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            var createTemplateHandler = new UpdateTemplateDetailsHandler(templateRepositoryMock.Object, validatorMock.Object);
            createTemplateHandler.Handle(command);

            validatorMock.Verify(x => x.Validate(command));
            templateRepositoryMock.Verify(x => x.Update(It.IsAny<Template>()));
        }
    }
}
