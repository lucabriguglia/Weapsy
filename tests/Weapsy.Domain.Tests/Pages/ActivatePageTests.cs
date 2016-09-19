using System.Linq;
using NUnit.Framework;
using Weapsy.Domain.Pages;
using Weapsy.Domain.Pages.Events;
using System;
using FluentValidation.Results;
using Weapsy.Domain.Pages.Commands;
using Moq;
using FluentValidation;

namespace Weapsy.Domain.Tests.Pages
{
    [TestFixture]
    public class ActivatePageTests
    {
        [Test]
        public void Should_call_validator()
        {
            var page = new Page();
            var command = new ActivatePage();
            var validatorMock = new Mock<IValidator<ActivatePage>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            page.Activate(command, validatorMock.Object);

            validatorMock.Verify(x => x.Validate(command));
        }

        [Test]
        public void Should_throw_exception_when_already_activated()
        {
            var page = new Page();
            var command = new ActivatePage();
            var validatorMock = new Mock<IValidator<ActivatePage>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            page.Activate(command, validatorMock.Object);

            Assert.Throws<Exception>(() => page.Activate(command, validatorMock.Object));
        }

        [Test]
        public void Should_set_page_status_to_activated()
        {
            var page = new Page();
            var command = new ActivatePage();
            var validatorMock = new Mock<IValidator<ActivatePage>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            page.Activate(command, validatorMock.Object);

            Assert.AreEqual(true, page.Status == PageStatus.Active);
        }

        [Test]
        public void Should_add_page_activated_event()
        {
            var page = new Page();
            var command = new ActivatePage();
            var validatorMock = new Mock<IValidator<ActivatePage>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            page.Activate(command, validatorMock.Object);

            var @event = page.Events.OfType<PageActivated>().SingleOrDefault();

            Assert.IsNotNull(@event);
        }

        [Test]
        public void Should_set_id_in_page_activated_event()
        {
            var page = new Page();
            var command = new ActivatePage();
            var validatorMock = new Mock<IValidator<ActivatePage>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            page.Activate(command, validatorMock.Object);

            var @event = page.Events.OfType<PageActivated>().SingleOrDefault();

            Assert.AreEqual(page.Id, @event.AggregateRootId);
        }

        [Test]
        public void Should_set_site_id_in_page_activated_event()
        {
            var page = new Page();
            var command = new ActivatePage();
            var validatorMock = new Mock<IValidator<ActivatePage>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            page.Activate(command, validatorMock.Object);

            var @event = page.Events.OfType<PageActivated>().SingleOrDefault();

            Assert.AreEqual(page.SiteId, @event.SiteId);
        }
    }
}