using System;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Themes;
using Weapsy.Domain.Themes.Commands;
using Weapsy.Domain.Themes.Handlers;

namespace Weapsy.Domain.Tests.Themes.Handlers
{
    [TestFixture]
    public class ActivateThemeHandlerTests
    {
        [Test]
        public void Should_throw_exception_when_theme_is_not_found()
        {
            var command = new ActivateTheme
            {
                Id = Guid.NewGuid()
            };

            var repositoryMock = new Mock<IThemeRepository>();
            repositoryMock.Setup(x => x.GetById(command.Id)).Returns((Theme)null);

            var activateThemeHandler = new ActivateThemeHandler(repositoryMock.Object);

            Assert.Throws<Exception>(() => activateThemeHandler.Handle(command));
        }

        [Test]
        public void Should_update_theme()
        {
            var command = new ActivateTheme
            {
                Id = Guid.NewGuid()
            };

            var themeMock = new Mock<Theme>();

            var repositoryMock = new Mock<IThemeRepository>();
            repositoryMock.Setup(x => x.GetById(command.Id)).Returns(themeMock.Object);

            var activateThemeHandler = new ActivateThemeHandler(repositoryMock.Object);

            activateThemeHandler.Handle(command);

            repositoryMock.Verify(x => x.Update(It.IsAny<Theme>()));
        }
    }
}
