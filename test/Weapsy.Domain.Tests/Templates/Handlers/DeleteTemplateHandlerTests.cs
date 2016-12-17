using System;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Templates;
using Weapsy.Domain.Templates.Commands;
using Weapsy.Domain.Templates.Handlers;

namespace Weapsy.Domain.Tests.Templates.Handlers
{
    [TestFixture]
    public class DeleteTemplateHandlerTests
    {
        [Test]
        public void Should_throw_exception_when_template_is_not_found()
        {
            var command = new DeleteTemplate
            {
                Id = Guid.NewGuid()
            };

            var repositoryMock = new Mock<ITemplateRepository>();
            repositoryMock.Setup(x => x.GetById(command.Id)).Returns((Template)null);

            var deleteTemplateHandler = new DeleteTemplateHandler(repositoryMock.Object);

            Assert.Throws<Exception>(() => deleteTemplateHandler.Handle(command));
        }

        [Test]
        public void Should_update_template()
        {
            var command = new DeleteTemplate
            {
                Id = Guid.NewGuid()
            };

            var templateMock = new Mock<Template>();

            var repositoryMock = new Mock<ITemplateRepository>();
            repositoryMock.Setup(x => x.GetById(command.Id)).Returns(templateMock.Object);

            var deleteTemplateHandler = new DeleteTemplateHandler(repositoryMock.Object);

            deleteTemplateHandler.Handle(command);

            repositoryMock.Verify(x => x.Update(It.IsAny<Template>()));
        }
    }
}
