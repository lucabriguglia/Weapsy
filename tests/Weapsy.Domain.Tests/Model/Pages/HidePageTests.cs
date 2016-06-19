using System.Linq;
using NUnit.Framework;
using Weapsy.Domain.Model.Pages;
using Weapsy.Domain.Model.Pages.Events;
using System;
using Weapsy.Domain.Model.Pages.Commands;
using Moq;
using FluentValidation;
using FluentValidation.Results;

namespace Weapsy.Domain.Tests.Pages
{
    [TestFixture]
    public class HidePageTests
    {
        private Page page;
        private Mock<IValidator<HidePage>> validatorMock;
        private HidePage command;
        private PageHidden @event;

        [SetUp]
        public void SetUp()
        {
            page = new Page();
            command = new HidePage();

            validatorMock = new Mock<IValidator<HidePage>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            page.Hide(command, validatorMock.Object);

            @event = page.Events.OfType<PageHidden>().SingleOrDefault();
        }

        [Test]
        public void Should_call_validator()
        {
            validatorMock.Verify(x => x.Validate(command));
        }

        [Test]
        public void Should_throw_exception_when_already_hidden()
        {
            Assert.Throws<Exception>(() => page.Hide(command, validatorMock.Object));
        }

        [Test]
        public void Should_throw_exception_when_deleted()
        {
            var deletePageCommand = new DeletePage
            {
                SiteId = page.SiteId,
                Id = page.Id
            };

            var deletePageValidatorMock = new Mock<IValidator<DeletePage>>();
            deletePageValidatorMock.Setup(x => x.Validate(deletePageCommand)).Returns(new ValidationResult());

            page.Delete(deletePageCommand, deletePageValidatorMock.Object);

            Assert.Throws<Exception>(() => page.Hide(command, validatorMock.Object));
        }

        [Test]
        public void Should_set_page_status_to_hidden()
        {
            Assert.AreEqual(true, page.Status == PageStatus.Hidden);
        }

        [Test]
        public void Should_add_page_hidden_event()
        {
            Assert.IsNotNull(@event);
        }

        [Test]
        public void Should_set_id_in_page_hidden_event()
        {
            Assert.AreEqual(page.Id, @event.AggregateRootId);
        }

        [Test]
        public void Should_set_site_id_in_page_hidden_event()
        {
            Assert.AreEqual(page.SiteId, @event.SiteId);
        }
    }
}
