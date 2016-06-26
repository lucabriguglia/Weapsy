using System;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Model.Pages;
using Weapsy.Domain.Model.Pages.Commands;
using Weapsy.Domain.Model.Pages.Events;
using System.Collections.Generic;

namespace Weapsy.Domain.Tests.Pages
{
    [TestFixture]
    public class UpdatePageDetailsTests
    {
        private UpdatePageDetails command;
        private Mock<IValidator<UpdatePageDetails>> validatorMock;
        private Page page;
        private PageDetailsUpdated @event;

        [SetUp]
        public void Setup()
        {
            var pageId = Guid.NewGuid();
            command = new UpdatePageDetails
            {
                SiteId = Guid.NewGuid(),
                Id = pageId,
                Name = "Name",
                Url = "url",
                Title = "Title",
                MetaDescription = "Meta Description",
                MetaKeywords = "Meta Keywords",
                PageLocalisations = new List<PageDetails.PageLocalisation>
                {
                    new PageDetails.PageLocalisation
                    {
                        LanguageId = Guid.NewGuid(),
                        Url = "url",
                        Title = "Title",
                        MetaDescription = "Meta Description",
                        MetaKeywords = "Meta Keywords"
                    }
                }
            };
            validatorMock = new Mock<IValidator<UpdatePageDetails>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());
            page = new Page();
            page.UpdateDetails(command, validatorMock.Object);
            @event = page.Events.OfType<PageDetailsUpdated>().SingleOrDefault();
        }

        [Test]
        public void Should_validate_command()
        {
            validatorMock.Verify(x => x.Validate(command));
        }

        [Test]
        public void Should_set_name()
        {
            Assert.AreEqual(command.Name, page.Name);
        }

        [Test]
        public void Should_set_url()
        {
            Assert.AreEqual(command.Url, page.Url);
        }

        [Test]
        public void Should_set_head_title()
        {
            Assert.AreEqual(command.Title, page.Title);
        }

        [Test]
        public void Should_set_meta_description()
        {
            Assert.AreEqual(command.MetaDescription, page.MetaDescription);
        }

        [Test]
        public void Should_set_meta_keywords()
        {
            Assert.AreEqual(command.MetaKeywords, page.MetaKeywords);
        }

        [Test]
        public void Should_set_localisation_language_id()
        {
            Assert.AreEqual(command.PageLocalisations[0].LanguageId, page.PageLocalisations.FirstOrDefault().LanguageId);
        }

        [Test]
        public void Should_set_localisation_url()
        {
            Assert.AreEqual(command.PageLocalisations[0].Url, page.PageLocalisations.FirstOrDefault().Url);
        }

        [Test]
        public void Should_set_localisation_head_title()
        {
            Assert.AreEqual(command.PageLocalisations[0].Title, page.PageLocalisations.FirstOrDefault().Title);
        }

        [Test]
        public void Should_set_localisation_meta_description()
        {
            Assert.AreEqual(command.PageLocalisations[0].MetaDescription, page.PageLocalisations.FirstOrDefault().MetaDescription);
        }

        [Test]
        public void Should_set_localisation_meta_keywords()
        {
            Assert.AreEqual(command.PageLocalisations[0].MetaKeywords, page.PageLocalisations.FirstOrDefault().MetaKeywords);
        }

        [Test]
        public void Should_add_page_details_updated_event()
        {
            Assert.IsNotNull(@event);
        }

        [Test]
        public void Should_set_site_id_in_page_details_updated_event()
        {
            Assert.AreEqual(page.SiteId, @event.SiteId);
        }

        [Test]
        public void Should_set_id_in_page_details_updated_event()
        {
            Assert.AreEqual(page.Id, @event.AggregateRootId);
        }

        [Test]
        public void Should_set_name_in_page_details_updated_event()
        {
            Assert.AreEqual(page.Name, @event.Name);
        }

        [Test]
        public void Should_set_url_in_page_details_updated_event()
        {
            Assert.AreEqual(page.Url, @event.Url);
        }

        [Test]
        public void Should_set_head_title_in_page_details_updated_event()
        {
            Assert.AreEqual(page.Title, @event.Title);
        }

        [Test]
        public void Should_set_meta_description_in_page_details_updated_event()
        {
            Assert.AreEqual(page.MetaDescription, @event.MetaDescription);
        }

        [Test]
        public void Should_set_meta_keywords_in_page_details_updated_event()
        {
            Assert.AreEqual(page.MetaKeywords, @event.MetaKeywords);
        }

        [Test]
        public void Should_set_page_localisations_in_page_details_updated_event()
        {
            Assert.AreEqual(page.PageLocalisations, @event.PageLocalisations);
        }

        [Test]
        public void Should_throw_exception_if_language_is_already_added_to_page_localisations()
        {
            var languageId = Guid.NewGuid();
            command.PageLocalisations.Add(new PageDetails.PageLocalisation
            {
                LanguageId = languageId
            });
            command.PageLocalisations.Add(new PageDetails.PageLocalisation
            {
                LanguageId = languageId
            });
            Assert.Throws<Exception>(() => page.UpdateDetails(command, validatorMock.Object));
        }
    }
}