using System;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Weapsy.Domain.Apps;
using Weapsy.Domain.Apps.Commands;

namespace Weapsy.Tests.Factories
{
    public static class AppFactory
    {
        public static App App()
        {
            return App(Guid.NewGuid(), "Name", "Description", "Folder");
        }

        public static App App(Guid id, string name, string description, string folder)
        {
            var command = new CreateApp
            {
                Id = id,
                Name = name,
                Description = description,
                Folder = folder
            };

            var validatorMock = new Mock<IValidator<CreateApp>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            return Domain.Apps.App.CreateNew(command, validatorMock.Object);
        }
    }
}
