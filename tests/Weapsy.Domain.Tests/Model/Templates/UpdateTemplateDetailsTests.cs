using System;
using System.Linq;
using NUnit.Framework;
using Moq;
using FluentValidation;
using FluentValidation.Results;
using Weapsy.Domain.Model.Templates;
using Weapsy.Domain.Model.Templates.Commands;
using Weapsy.Domain.Model.Templates.Events;

namespace Weapsy.Domain.Tests.Templates
{
    [TestFixture]
    public class UpdateTemplateDetailsTests
    {
        private UpdateTemplateDetails command;
        private Mock<IValidator<UpdateTemplateDetails>> validatorMock;
        private Template template;
        private TemplateDetailsUpdated @event;

        [SetUp]
        public void Setup()
        {
            command = new UpdateTemplateDetails
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Description = "Description",
                ViewName = "viewName"
            };            
            validatorMock = new Mock<IValidator<UpdateTemplateDetails>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());
            template = new Template();
            template.UpdateDetails(command, validatorMock.Object);
            @event = template.Events.OfType<TemplateDetailsUpdated>().SingleOrDefault();
        }

        [Test]
        public void Should_validate_command()
        {
            validatorMock.Verify(x => x.Validate(command));
        }

        [Test]
        public void Should_set_name()
        {
            Assert.AreEqual(command.Name, template.Name);
        }

        [Test]
        public void Should_set_description()
        {
            Assert.AreEqual(command.Description, template.Description);
        }

        [Test]
        public void Should_set_viewName()
        {
            Assert.AreEqual(command.ViewName, template.ViewName);
        }

        [Test]
        public void Should_add_template_details_updated_event()
        {
            Assert.IsNotNull(@event);
        }

        [Test]
        public void Should_set_id_in_template_details_updated_event()
        {
            Assert.AreEqual(template.Id, @event.AggregateRootId);
        }

        [Test]
        public void Should_set_name_in_template_details_updated_event()
        {
            Assert.AreEqual(template.Name, @event.Name);
        }

        [Test]
        public void Should_set_description_in_template_details_updated_event()
        {
            Assert.AreEqual(template.Description, @event.Description);
        }

        [Test]
        public void Should_set_view_name_in_template_details_updated_event()
        {
            Assert.AreEqual(template.ViewName, @event.ViewName);
        }
    }
}
