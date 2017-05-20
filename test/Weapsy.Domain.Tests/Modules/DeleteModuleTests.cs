using System.Linq;
using NUnit.Framework;
using Weapsy.Domain.Modules;
using Weapsy.Domain.Modules.Events;
using System;
using Moq;
using FluentValidation;
using Weapsy.Domain.Modules.Commands;
using FluentValidation.Results;

namespace Weapsy.Domain.Tests.Modules
{
    [TestFixture]
    public class DeleteModuleTests
    {
        [Test]
        public void Should_throw_exception_when_already_deleted()
        {
            var command = new DeleteModule
            {
                SiteId = Guid.NewGuid(),
                Id = Guid.NewGuid()
            };

            var validatorMock = new Mock<IValidator<DeleteModule>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            var module = new Module();

            module.Delete(command, validatorMock.Object);
            
            Assert.Throws<Exception>(() => module.Delete(command, validatorMock.Object));
        }

        [Test]
        public void Should_call_validator()
        {
            var command = new DeleteModule
            {
                SiteId = Guid.NewGuid(),
                Id = Guid.NewGuid()
            };

            var validatorMock = new Mock<IValidator<DeleteModule>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            var module = new Module();

            module.Delete(command, validatorMock.Object);

            validatorMock.Verify(x => x.Validate(command));
        }

        [Test]
        public void Should_set_module_status_to_deleted()
        {
            var command = new DeleteModule
            {
                SiteId = Guid.NewGuid(),
                Id = Guid.NewGuid()
            };

            var validatorMock = new Mock<IValidator<DeleteModule>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            var module = new Module();

            module.Delete(command, validatorMock.Object);

            Assert.AreEqual(true, module.Status == ModuleStatus.Deleted);
        }

        [Test]
        public void Should_add_module_deleted_event()
        {
            var command = new DeleteModule
            {
                SiteId = Guid.NewGuid(),
                Id = Guid.NewGuid()
            };

            var validatorMock = new Mock<IValidator<DeleteModule>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            var module = new Module();

            module.Delete(command, validatorMock.Object);

            var @event = module.Events.OfType<ModuleDeleted>().SingleOrDefault();

            Assert.IsNotNull(@event);
        }

        [Test]
        public void Should_set_id_in_module_deleted_event()
        {
            var command = new DeleteModule
            {
                SiteId = Guid.NewGuid(),
                Id = Guid.NewGuid()
            };

            var validatorMock = new Mock<IValidator<DeleteModule>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            var module = new Module();

            module.Delete(command, validatorMock.Object);

            var @event = module.Events.OfType<ModuleDeleted>().SingleOrDefault();

            Assert.AreEqual(module.Id, @event.AggregateRootId);
        }

        [Test]
        public void Should_set_site_id_in_module_deleted_event()
        {
            var command = new DeleteModule
            {
                SiteId = Guid.NewGuid(),
                Id = Guid.NewGuid()
            };

            var validatorMock = new Mock<IValidator<DeleteModule>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            var module = new Module();

            module.Delete(command, validatorMock.Object);

            var @event = module.Events.OfType<ModuleDeleted>().SingleOrDefault();

            Assert.AreEqual(module.SiteId, @event.SiteId);
        }
    }
}
