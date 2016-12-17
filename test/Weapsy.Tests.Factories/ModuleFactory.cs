using System;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Weapsy.Domain.Modules;
using Weapsy.Domain.Modules.Commands;

namespace Weapsy.Tests.Factories
{
    public static class ModuleFactory
    {
        public static Module Module()
        {
            return Module(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "My Module");
        }

        public static Module Module(Guid siteId, Guid moduleTypeId, Guid id, string title)
        {
            var command = new CreateModule
            {
                SiteId = siteId,
                ModuleTypeId = moduleTypeId,
                Id = id,
                Title = title
            };

            var validatorMock = new Mock<IValidator<CreateModule>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            return Domain.Modules.Module.CreateNew(command, validatorMock.Object);
        }
    }
}
