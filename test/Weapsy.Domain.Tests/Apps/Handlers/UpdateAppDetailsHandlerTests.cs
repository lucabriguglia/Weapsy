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
using Weapsy.Tests.Factories;

namespace Weapsy.Domain.Tests.Apps.Handlers
{
    [TestFixture]
    public class UpdateAppDetailsHandlerTests
    {
        private UpdateAppDetails _command;
        private Mock<IAppRepository> _appRepositoryMock;
        private Mock<IValidator<UpdateAppDetails>> _validatorMock;
        private UpdateAppDetailsHandler _sut;
        private App _app;
        private App _updatedApp;

        [SetUp]
        public void SetUp()
        {
            _app = AppFactory.CreateApp();

            _command = new UpdateAppDetails
            {
                Id = _app.Id,
                Name = "New Name",
                Description = "New Description",
                Folder = "New Folder"
            };

            _appRepositoryMock = new Mock<IAppRepository>();
            _appRepositoryMock
                .Setup(x => x.GetById(_command.Id))
                .Returns(_app);
            _appRepositoryMock
                .Setup(x => x.UpdateAsync(It.IsAny<App>()))
                .Callback<App>(app => _updatedApp = app)
                .Returns(Task.CompletedTask);

            _validatorMock = new Mock<IValidator<UpdateAppDetails>>();
            _validatorMock
                .Setup(x => x.ValidateAsync(_command, CancellationToken.None))
                .ReturnsAsync(new ValidationResult());

            _sut = new UpdateAppDetailsHandler(_appRepositoryMock.Object, _validatorMock.Object);
        }

        [Test]
        public void Should_throw_application_exception_when_app_is_not_found()
        {
            _appRepositoryMock
                .Setup(x => x.GetById(_command.Id))
                .Returns(default(App));

            Assert.ThrowsAsync<ApplicationException>(async () => await _sut.HandleAsync(_command));
        }

        [Test]
        public void Should_throw_application_exception_when_validation_fails()
        {
            _validatorMock
                .Setup(x => x.ValidateAsync(_command, CancellationToken.None))
                .ReturnsAsync(new ValidationResult(new List<ValidationFailure>
                {
                    new ValidationFailure("Name", "Name Error")
                }));

            Assert.ThrowsAsync<ApplicationException>(async () => await _sut.HandleAsync(_command));
        }

        [Test]
        public async Task Should_validate_command()
        {
            await _sut.HandleAsync(_command);

            _validatorMock.Verify(x => x.ValidateAsync(_command, CancellationToken.None), Times.Once);
        }

        [Test]
        public async Task Should_update_app()
        {
            await _sut.HandleAsync(_command);

            Assert.NotNull(_updatedApp.Events.OfType<AppDetailsUpdated>().FirstOrDefault());
        }

        [Test]
        public async Task Should_return_updated_app()
        {
            var actual = await _sut.HandleAsync(_command);

            Assert.AreEqual(_updatedApp, actual);
        }
    }
}
