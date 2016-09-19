using System.Linq;
using NUnit.Framework;
using Weapsy.Domain.Pages;
using Weapsy.Domain.Pages.Events;
using System;
using Weapsy.Domain.Pages.Commands;
using Moq;
using FluentValidation;
using FluentValidation.Results;

namespace Weapsy.Domain.Tests.Pages
{
    [TestFixture]
    public class HidePageTests
    {
        private Page _page;
        private Mock<IValidator<HidePage>> _validatorMock;
        private HidePage _command;
        private PageHidden _event;

        [SetUp]
        public void SetUp()
        {
            _page = new Page();
            _command = new HidePage();

            _validatorMock = new Mock<IValidator<HidePage>>();
            _validatorMock.Setup(x => x.Validate(_command)).Returns(new ValidationResult());

            _page.Hide(_command, _validatorMock.Object);

            _event = _page.Events.OfType<PageHidden>().SingleOrDefault();
        }

        [Test]
        public void Should_call_validator()
        {
            _validatorMock.Verify(x => x.Validate(_command));
        }

        [Test]
        public void Should_throw_exception_when_already_hidden()
        {
            Assert.Throws<Exception>(() => _page.Hide(_command, _validatorMock.Object));
        }

        [Test]
        public void Should_throw_exception_when_deleted()
        {
            var deletePageCommand = new DeletePage
            {
                SiteId = _page.SiteId,
                Id = _page.Id
            };

            var deletePageValidatorMock = new Mock<IValidator<DeletePage>>();
            deletePageValidatorMock.Setup(x => x.Validate(deletePageCommand)).Returns(new ValidationResult());

            _page.Delete(deletePageCommand, deletePageValidatorMock.Object);

            Assert.Throws<Exception>(() => _page.Hide(_command, _validatorMock.Object));
        }

        [Test]
        public void Should_set_page_status_to_hidden()
        {
            Assert.AreEqual(true, _page.Status == PageStatus.Hidden);
        }

        [Test]
        public void Should_add_page_hidden_event()
        {
            Assert.IsNotNull(_event);
        }

        [Test]
        public void Should_set_id_in_page_hidden_event()
        {
            Assert.AreEqual(_page.Id, _event.AggregateRootId);
        }

        [Test]
        public void Should_set_site_id_in_page_hidden_event()
        {
            Assert.AreEqual(_page.SiteId, _event.SiteId);
        }
    }
}
