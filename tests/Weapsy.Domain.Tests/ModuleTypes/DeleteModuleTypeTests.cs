using System.Linq;
using NUnit.Framework;
using Weapsy.Domain.ModuleTypes;
using Weapsy.Domain.ModuleTypes.Events;
using System;
using Moq;
using FluentValidation;
using Weapsy.Domain.ModuleTypes.Commands;
using FluentValidation.Results;

namespace Weapsy.Domain.Tests.ModuleTypes
{
    [TestFixture]
    public class DeleteModuleTypeTests
    {
        [Test]
        public void Should_throw_exception_when_already_deleted()
        {
            var command = new DeleteModuleType();
            var moduleType = new ModuleType();
            var validatorMock = new Mock<IValidator<DeleteModuleType>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());
            
            moduleType.Delete(command, validatorMock.Object);
            
            Assert.Throws<Exception>(() => moduleType.Delete(command, validatorMock.Object));
        }

        [Test]
        public void Should_validate_command()
        {
            var command = new DeleteModuleType();
            var moduleType = new ModuleType();
            var validatorMock = new Mock<IValidator<DeleteModuleType>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            moduleType.Delete(command, validatorMock.Object);

            validatorMock.Verify(x => x.Validate(command));
        }

        [Test]
        public void Should_set_module_type_status_to_deleted()
        {
            var command = new DeleteModuleType();
            var moduleType = new ModuleType();
            var validatorMock = new Mock<IValidator<DeleteModuleType>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            moduleType.Delete(command, validatorMock.Object);

            Assert.AreEqual(true, moduleType.Status == ModuleTypeStatus.Deleted);
        }

        [Test]
        public void Should_add_module_type_deleted_event()
        {
            var command = new DeleteModuleType();
            var moduleType = new ModuleType();
            var validatorMock = new Mock<IValidator<DeleteModuleType>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            moduleType.Delete(command, validatorMock.Object);

            var @event = moduleType.Events.OfType<ModuleTypeDeleted>().SingleOrDefault();

            Assert.IsNotNull(@event);
        }

        [Test]
        public void Should_set_id_in_module_type_deleted_event()
        {
            var command = new DeleteModuleType();
            var moduleType = new ModuleType();
            var validatorMock = new Mock<IValidator<DeleteModuleType>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            moduleType.Delete(command, validatorMock.Object);

            var @event = moduleType.Events.OfType<ModuleTypeDeleted>().SingleOrDefault();

            Assert.AreEqual(moduleType.Id, @event.AggregateRootId);
        }
    }
}
