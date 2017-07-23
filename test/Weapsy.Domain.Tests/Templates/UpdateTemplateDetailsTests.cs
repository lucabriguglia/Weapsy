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
    public class UpdateTemplateDetailsTests
    {
        private UpdateTemplateDetails _command;
        private Mock<IValidator<UpdateTemplateDetails>> _validatorMock;
        private Template _template;
        private TemplateDetailsUpdated _event;

        [SetUp]
        public void Setup()
        {
            _command = new UpdateTemplateDetails
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Description = "Description",
                ViewName = "viewName"
            };            
            _validatorMock = new Mock<IValidator<UpdateTemplateDetails>>();
            _validatorMock.Setup(x => x.Validate(_command)).Returns(new ValidationResult());
            _template = new Template();
            _template.UpdateDetails(_command, _validatorMock.Object);
            _event = _template.Events.OfType<TemplateDetailsUpdated>().SingleOrDefault();
        }

        [Test]
        public void Should_validate_command()
        {
            _validatorMock.Verify(x => x.Validate(_command));
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
        public void Should_set_type()
        {
            Assert.AreEqual(_command.Type, _template.Type);
        }

        [Test]
        public void Should_add_template_details_updated_event()
        {
            Assert.IsNotNull(_event);
        }

        [Test]
        public void Should_set_id_in_template_details_updated_event()
        {
            Assert.AreEqual(_template.Id, _event.AggregateRootId);
        }

        [Test]
        public void Should_set_name_in_template_details_updated_event()
        {
            Assert.AreEqual(_template.Name, _event.Name);
        }

        [Test]
        public void Should_set_description_in_template_details_updated_event()
        {
            Assert.AreEqual(_template.Description, _event.Description);
        }

        [Test]
        public void Should_set_view_name_in_template_details_updated_event()
        {
            Assert.AreEqual(_template.ViewName, _event.ViewName);
        }

        [Test]
        public void Should_set_type_in_template_details_updated_event()
        {
            Assert.AreEqual(_template.Type, _event.Type);
        }
    }
}
