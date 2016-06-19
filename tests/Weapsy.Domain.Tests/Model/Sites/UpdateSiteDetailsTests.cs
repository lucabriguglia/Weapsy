using System;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Model.Sites;
using Weapsy.Domain.Model.Sites.Commands;
using Weapsy.Domain.Model.Sites.Events;
using System.Collections.Generic;

namespace Weapsy.Domain.Tests.Sites
{
    [TestFixture]
    public class UpdateSiteDetailsTests
    {
        private UpdateSiteDetails command;
        private Mock<IValidator<UpdateSiteDetails>> validatorMock;
        private Site site;
        private SiteDetailsUpdated @event;

        [SetUp]
        public void Setup()
        {
            var siteId = Guid.NewGuid();
            command = new UpdateSiteDetails
            {
                SiteId = Guid.NewGuid(),
                Url = "url",
                Title = "Title",
                MetaDescription = "Meta Description",
                MetaKeywords = "Meta Keywords",
                SiteLocalisations = new List<UpdateSiteDetails.SiteLocalisation>
                {
                    new UpdateSiteDetails.SiteLocalisation
                    {
                        LanguageId = Guid.NewGuid(),
                        Title = "Title",
                        MetaDescription = "Meta Description",
                        MetaKeywords = "Meta Keywords"
                    }
                }
            };
            validatorMock = new Mock<IValidator<UpdateSiteDetails>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());
            site = new Site();
            site.UpdateDetails(command, validatorMock.Object);
            @event = site.Events.OfType<SiteDetailsUpdated>().SingleOrDefault();
        }

        [Test]
        public void Should_validate_command()
        {
            validatorMock.Verify(x => x.Validate(command));
        }

        [Test]
        public void Should_set_url()
        {
            Assert.AreEqual(command.Url, site.Url);
        }

        [Test]
        public void Should_set_head_title()
        {
            Assert.AreEqual(command.Title, site.Title);
        }

        [Test]
        public void Should_set_meta_description()
        {
            Assert.AreEqual(command.MetaDescription, site.MetaDescription);
        }

        [Test]
        public void Should_set_meta_keywords()
        {
            Assert.AreEqual(command.MetaKeywords, site.MetaKeywords);
        }

        [Test]
        public void Should_set_localisations()
        {
            Assert.AreEqual(command.SiteLocalisations.Count, site.SiteLocalisations.Count);
        }

        [Test]
        public void Should_set_localisation_language_id()
        {
            Assert.AreEqual(command.SiteLocalisations[0].LanguageId, site.SiteLocalisations.FirstOrDefault().LanguageId);
        }

        [Test]
        public void Should_set_localisation_head_title()
        {
            Assert.AreEqual(command.SiteLocalisations[0].Title, site.SiteLocalisations.FirstOrDefault().Title);
        }

        [Test]
        public void Should_set_localisation_meta_description()
        {
            Assert.AreEqual(command.SiteLocalisations[0].MetaDescription, site.SiteLocalisations.FirstOrDefault().MetaDescription);
        }

        [Test]
        public void Should_set_localisation_meta_keywords()
        {
            Assert.AreEqual(command.SiteLocalisations[0].MetaKeywords, site.SiteLocalisations.FirstOrDefault().MetaKeywords);
        }

        [Test]
        public void Should_add_site_details_updated_event()
        {
            Assert.IsNotNull(@event);
        }

        [Test]
        public void Should_set_id_in_site_details_updated_event()
        {
            Assert.AreEqual(site.Id, @event.AggregateRootId);
        }

        [Test]
        public void Should_set_url_in_site_details_updated_event()
        {
            Assert.AreEqual(site.Url, @event.Url);
        }

        [Test]
        public void Should_set_head_title_in_site_details_updated_event()
        {
            Assert.AreEqual(site.Title, @event.Title);
        }

        [Test]
        public void Should_set_meta_description_in_site_details_updated_event()
        {
            Assert.AreEqual(site.MetaDescription, @event.MetaDescription);
        }

        [Test]
        public void Should_set_meta_keywords_in_site_details_updated_event()
        {
            Assert.AreEqual(site.MetaKeywords, @event.MetaKeywords);
        }

        [Test]
        public void Should_set_site_localisations_in_site_details_updated_event()
        {
            Assert.AreEqual(site.SiteLocalisations, @event.SiteLocalisations);
        }

        [Test]
        public void Should_throw_exception_if_language_is_already_added_to_site_localisations()
        {
            var languageId = Guid.NewGuid();
            command.SiteLocalisations.Add(new UpdateSiteDetails.SiteLocalisation
            {
                LanguageId = languageId
            });
            command.SiteLocalisations.Add(new UpdateSiteDetails.SiteLocalisation
            {
                LanguageId = languageId
            });
            Assert.Throws<Exception>(() => site.UpdateDetails(command, validatorMock.Object));
        }
    }
}