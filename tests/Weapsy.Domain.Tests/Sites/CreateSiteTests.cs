using System;
using System.Linq;
using NUnit.Framework;
using Moq;
using FluentValidation;
using FluentValidation.Results;
using Weapsy.Domain.Sites;
using Weapsy.Domain.Sites.Commands;
using Weapsy.Domain.Sites.Events;

namespace Weapsy.Domain.Tests.Sites
{
    [TestFixture]
    public class CreateSiteTests
    {
        private CreateSite _command;
        private Mock<IValidator<CreateSite>> _validatorMock;
        private Site _site;
        private SiteCreated _event;

        [SetUp]
        public void Setup()
        {
            _command = new CreateSite
            {
                Id = Guid.NewGuid(),
                Name = "Name"
            };
            _validatorMock = new Mock<IValidator<CreateSite>>();
            _validatorMock.Setup(x => x.Validate(_command)).Returns(new ValidationResult());
            _site = Site.CreateNew(_command, _validatorMock.Object);
            _event = _site.Events.OfType<SiteCreated>().SingleOrDefault();
        }

        [Test]
        public void Should_validate_command()
        {
            _validatorMock.Verify(x => x.Validate(_command));
        }

        [Test]
        public void Should_set_id()
        {
            Assert.AreEqual(_command.Id, _site.Id);
        }

        [Test]
        public void Should_set_name()
        {
            Assert.AreEqual(_command.Name, _site.Name);
        }

        [Test]
        public void Should_set_status_to_active()
        {
            Assert.AreEqual(SiteStatus.Active, _site.Status);
        }

        [Test]
        public void Should_add_site_created_event()
        {
            Assert.IsNotNull(_event);
        }

        [Test]
        public void Should_set_id_in_site_created_event()
        {
            Assert.AreEqual(_site.Id, _event.AggregateRootId);
        }

        [Test]
        public void Should_set_name_in_site_created_event()
        {
            Assert.AreEqual(_site.Name, _event.Name);
        }

        [Test]
        public void Should_set_status_in_site_created_event()
        {
            Assert.AreEqual(_site.Status, _event.Status);
        }
    }
}
