using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Apps;
using Weapsy.Domain.Apps.Commands;
using Weapsy.Domain.Apps.Commands.Handlers;
using Weapsy.Domain.Apps.Events;

namespace Weapsy.Domain.Tests.Apps.Handlers
{
    [TestFixture]
    public class CreateAppHandlerTests
    {
        private CreateApp _command;
        private Mock<IAppRepository> _appRepositoryMock;
        private Mock<IValidator<CreateApp>> _validatorMock;
        private CreateAppHandler _sut;
        private App _savedApp;

        [SetUp]
        public void SetUp()
        {
            _command = new CreateApp
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Description = "Description",
                Folder = "Folder"
            };

            _appRepositoryMock = new Mock<IAppRepository>();
            _appRepositoryMock
                .Setup(x => x.CreateAsync(It.IsAny<App>()))
                .Callback<App>(app => _savedApp = app)
                .Returns(Task.CompletedTask);

            _validatorMock = new Mock<IValidator<CreateApp>>();
            _validatorMock
                .Setup(x => x.ValidateAsync(_command, CancellationToken.None))
                .ReturnsAsync(new ValidationResult());

            _sut = new CreateAppHandler(_appRepositoryMock.Object, _validatorMock.Object);
        }

        [Test]
        public void Should_throw_validation_exception_when_validation_fails()
        {
            _validatorMock
                .Setup(x => x.ValidateAsync(_command, CancellationToken.None))
                .ReturnsAsync(new ValidationResult(new List<ValidationFailure>
                {
                    new ValidationFailure("Id", "Id Error")
                }));

            Assert.ThrowsAsync<Exception>(async () => await _sut.HandleAsync(_command));
        }

        [Test]
        public async Task Should_validate_command()
        {
            await _sut.HandleAsync(_command);

            _validatorMock.Verify(x => x.ValidateAsync(_command, CancellationToken.None), Times.Once);
        }

        [Test]
        public async Task Should_save_new_app()
        {
            await _sut.HandleAsync(_command);

            Assert.NotNull(_savedApp.Events.OfType<AppCreated>().SingleOrDefault());
        }

        [Test]
        public async Task Should_return_saved_app()
        {
            var actual = await _sut.HandleAsync(_command);

            Assert.AreEqual(_savedApp, actual);
        }
    }
}
