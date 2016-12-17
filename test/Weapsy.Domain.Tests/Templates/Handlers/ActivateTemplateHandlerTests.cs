using System;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Templates;
using Weapsy.Domain.Templates.Commands;
using Weapsy.Domain.Templates.Handlers;

namespace Weapsy.Domain.Tests.Templates.Handlers
{
    [TestFixture]
    public class ActivateTemplateHandlerTests
    {
        [Test]
        public void Should_throw_exception_when_template_is_not_found()
        {
            var command = new ActivateTemplate
            {
                Id = Guid.NewGuid()
            };

            var repositoryMock = new Mock<ITemplateRepository>();
            repositoryMock.Setup(x => x.GetById(command.Id)).Returns((Template)null);

            var activateTemplateHandler = new ActivateTemplateHandler(repositoryMock.Object);

            Assert.Throws<Exception>(() => activateTemplateHandler.Handle(command));
        }

        [Test]
        public void Should_update_template()
        {
            var command = new ActivateTemplate
            {
                Id = Guid.NewGuid()
            };

            var templateMock = new Mock<Template>();

            var repositoryMock = new Mock<ITemplateRepository>();
            repositoryMock.Setup(x => x.GetById(command.Id)).Returns(templateMock.Object);

            var activateTemplateHandler = new ActivateTemplateHandler(repositoryMock.Object);

            activateTemplateHandler.Handle(command);

            repositoryMock.Verify(x => x.Update(It.IsAny<Template>()));
        }
    }
}
