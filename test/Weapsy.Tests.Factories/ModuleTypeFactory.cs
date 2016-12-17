using System;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Weapsy.Domain.ModuleTypes;
using Weapsy.Domain.ModuleTypes.Commands;

namespace Weapsy.Tests.Factories
{
    public static class ModuleTypeFactory
    {
        public static ModuleType ModuleType()
        {
            return ModuleType(Guid.NewGuid(), "Module Type Name", "Module Type Title", "Module Type Description");
        }

        public static ModuleType ModuleType(Guid id, string name, string title, string description)
        {
            var command = new CreateModuleType
            {
                Id = id,
                Name = name,
                Title = title,
                Description = description
            };

            var validatorMock = new Mock<IValidator<CreateModuleType>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            return Domain.ModuleTypes.ModuleType.CreateNew(command, validatorMock.Object);
        }
    }
}
