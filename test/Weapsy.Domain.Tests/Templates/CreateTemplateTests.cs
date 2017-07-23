using System;
using System.Linq;
using NUnit.Framework;
using Moq;
using FluentValidation;
using FluentValidation.Results;
using Weapsy.Domain.Templates;
using Weapsy.Domain.Templates.Commands;
using Weapsy.Domain.Templates.Events;

namespace Weapsy.Domain.Tests.Templates
{
    [TestFixture]
    public class CreateTemplateTests
    {
        private CreateTemplate _command;
        private Mock<IValidator<CreateTemplate>> _validatorMock;
        private Template _template;
        private TemplateCreated _event;

        [SetUp]
        public void Setup()
        {
            _command = new CreateTemplate
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Description = "Description",
                ViewName = "ViewName"
            };            
            _validatorMock = new Mock<IValidator<CreateTemplate>>();
            _validatorMock.Setup(x => x.Validate(_command)).Returns(new ValidationResult());
            _template = Template.CreateNew(_command, _validatorMock.Object);
            _event = _template.Events.OfType<TemplateCreated>().SingleOrDefault();
        }

        [Test]
        public void Should_validate_command()
        {
            _validatorMock.Verify(x => x.Validate(_command));
        }

        [Test]
        public void Should_set_id()
        {
            Assert.AreEqual(_command.Id, _template.Id);
        }

        [Test]
        public void Should_set_name()
        {
            Assert.AreEqual(_command.Name, _template.Name);
        }

        [Test]
        public void Should_set_description()
        {
            Assert.AreEqual(_command.Description, _template.Description);
        }

        [Test]
        public void Should_set_viewName()
        {
            Assert.AreEqual(_command.ViewName, _template.ViewName);
        }

        [Test]
        public void Should_set_status_to_hidden()
        {
            Assert.AreEqual(TemplateStatus.Hidden, _template.Status);
        }

        [Test]
        public void Should_set_type()
        {
            Assert.AreEqual(_command.Type, _template.Type);
        }

        [Test]
        public void Should_add_template_created_event()
        {
            Assert.IsNotNull(_event);
        }

        [Test]
        public void Should_set_id_in_template_created_event()
        {
            Assert.AreEqual(_template.Id, _event.AggregateRootId);
        }

        [Test]
        public void Should_set_name_in_template_created_event()
        {
            Assert.AreEqual(_template.Name, _event.Name);
        }

        [Test]
        public void Should_set_description_in_template_created_event()
        {
            Assert.AreEqual(_template.Description, _event.Description);
        }

        [Test]
        public void Should_set_view_name_in_template_created_event()
        {
            Assert.AreEqual(_template.ViewName, _event.ViewName);
        }

        [Test]
        public void Should_set_status_in_template_created_event()
        {
            Assert.AreEqual(_template.Status, _event.Status);
        }

        [Test]
        public void Should_set_type_in_template_created_event()
        {
            Assert.AreEqual(_template.Type, _event.Type);
        }
    }
}
