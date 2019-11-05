using AutoFixture;
using FluentValidation;
using FluentValidation.Results;
using Kledex.Commands;
using Moq;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;
using Weapsy.Domain.Models.Sites;
using Weapsy.Domain.Models.Sites.Commands;

namespace Weapsy.Domain.Handlers.Tests
{
    [TestFixture]
    public class CreateSiteHandlerTests
    {
        private CreateSite _command;
        private Site _savedSite;
        private CommandResponse _response;

        private Mock<ISiteRepository> _repositoryMock;
        private Mock<IValidator<CreateSite>> _validatorMock;
        private ICommandHandlerAsync<CreateSite> _sut;

        [SetUp]
        public async Task SetUp()
        {
            _command = new Fixture().Create<CreateSite>();

            _validatorMock = new Mock<IValidator<CreateSite>>();
            _validatorMock
                .Setup(x => x.ValidateAsync(_command, CancellationToken.None))
                .ReturnsAsync(new ValidationResult());

            _repositoryMock = new Mock<ISiteRepository>();
            _repositoryMock
                .Setup(x => x.CreateAsync(It.IsAny<Site>()))
                .Callback<Site>(x => _savedSite = x)
                .Returns(Task.CompletedTask);

            _sut = new CreateSiteHandler(_repositoryMock.Object, _validatorMock.Object);

            _response = await _sut.HandleAsync(_command);
        }

        [Test]
        public void ValidatesCommand()
        {
            _validatorMock.Verify(x => x.ValidateAsync(_command, CancellationToken.None), Times.Once);
        }

        [Test]
        public void SavesSite()
        {
            Assert.NotNull(_savedSite);
        }

        [Test]
        public void ReturnsEvents()
        {
            Assert.AreEqual(_response.Events, _savedSite.Events);
        }
    }
}
