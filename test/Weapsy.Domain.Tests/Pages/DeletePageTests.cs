using System.Linq;
using NUnit.Framework;
using Weapsy.Domain.Pages;
using Weapsy.Domain.Pages.Events;
using System;
using FluentValidation;
using Moq;
using FluentValidation.Results;
using Weapsy.Domain.Pages.Commands;

namespace Weapsy.Domain.Tests.Pages
{
    [TestFixture]
    public class DeletePageTests
    {
        [Test]
        public void Should_call_validator()
        {
            var page = new Page();
            var command = new DeletePage();
            var validatorMock = new Mock<IValidator<DeletePage>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            page.Delete(command, validatorMock.Object);

            validatorMock.Verify(x => x.Validate(command));
        }

        [Test]
        public void Should_throw_exception_when_already_deleted()
        {
            var page = new Page();
            var command = new DeletePage();
            var validatorMock = new Mock<IValidator<DeletePage>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            page.Delete(command, validatorMock.Object);

            Assert.Throws<Exception>(() => page.Delete(command, validatorMock.Object));
        }

        [Test]
        public void Should_set_page_status_to_deleted()
        {
            var page = new Page();
            var command = new DeletePage();
            var validatorMock = new Mock<IValidator<DeletePage>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            page.Delete(command, validatorMock.Object);

            Assert.AreEqual(true, page.Status == PageStatus.Deleted);
        }

        [Test]
        public void Should_add_page_deleted_event()
        {
            var page = new Page();
            var command = new DeletePage();
            var validatorMock = new Mock<IValidator<DeletePage>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            page.Delete(command, validatorMock.Object);

            var @event = page.Events.OfType<PageDeleted>().SingleOrDefault();

            Assert.IsNotNull(@event);
        }

        [Test]
        public void Should_set_id_in_page_deleted_event()
        {
            var page = new Page();
            var command = new DeletePage();
            var validatorMock = new Mock<IValidator<DeletePage>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            page.Delete(command, validatorMock.Object);

            var @event = page.Events.OfType<PageDeleted>().SingleOrDefault();

            Assert.AreEqual(page.Id, @event.AggregateRootId);
        }

        [Test]
        public void Should_set_site_id_in_page_deleted_event()
        {
            var page = new Page();
            var command = new DeletePage();
            var validatorMock = new Mock<IValidator<DeletePage>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            page.Delete(command, validatorMock.Object);

            var @event = page.Events.OfType<PageDeleted>().SingleOrDefault();

            Assert.AreEqual(page.SiteId, @event.SiteId);
        }
    }
}
