using System;
using System.Linq;
using NUnit.Framework;
using Moq;
using FluentValidation;
using FluentValidation.Results;
using Weapsy.Domain.Model.Sites;
using Weapsy.Domain.Model.Sites.Commands;
using Weapsy.Domain.Model.Sites.Events;

namespace Weapsy.Domain.Tests.Sites
{
    [TestFixture]
    public class CreateSiteTests
    {
        private CreateSite command;
        private Mock<IValidator<CreateSite>> validatorMock;
        private Site site;
        private SiteCreated @event;

        [SetUp]
        public void Setup()
        {
            command = new CreateSite
            {
                Id = Guid.NewGuid(),
                Name = "Name"
            };
            validatorMock = new Mock<IValidator<CreateSite>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());
            site = Site.CreateNew(command, validatorMock.Object);
            @event = site.Events.OfType<SiteCreated>().SingleOrDefault();
        }

        [Test]
        public void Should_validate_command()
        {
            validatorMock.Verify(x => x.Validate(command));
        }

        [Test]
        public void Should_set_id()
        {
            Assert.AreEqual(command.Id, site.Id);
        }

        [Test]
        public void Should_set_name()
        {
            Assert.AreEqual(command.Name, site.Name);
        }

        [Test]
        public void Should_set_status_to_active()
        {
            Assert.AreEqual(SiteStatus.Active, site.Status);
        }

        [Test]
        public void Should_add_site_created_event()
        {
            Assert.IsNotNull(@event);
        }

        [Test]
        public void Should_set_id_in_site_created_event()
        {
            Assert.AreEqual(site.Id, @event.AggregateRootId);
        }

        [Test]
        public void Should_set_name_in_site_created_event()
        {
            Assert.AreEqual(site.Name, @event.Name);
        }

        [Test]
        public void Should_set_status_in_site_created_event()
        {
            Assert.AreEqual(site.Status, @event.Status);
        }
    }
}
