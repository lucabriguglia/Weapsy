using System;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Sites;
using Weapsy.Domain.Sites.Commands;
using Weapsy.Domain.Sites.Events;
using System.Collections.Generic;

namespace Weapsy.Domain.Tests.Sites
{
    [TestFixture]
    public class UpdateSiteDetailsTests
    {
        private UpdateSiteDetails _command;
        private Mock<IValidator<UpdateSiteDetails>> _validatorMock;
        private Site _site;
        private SiteDetailsUpdated _event;

        [SetUp]
        public void Setup()
        {
            var siteId = Guid.NewGuid();
            _command = new UpdateSiteDetails
            {
                SiteId = Guid.NewGuid(),
                Url = "url",
                Title = "Title",
                MetaDescription = "Meta Description",
                MetaKeywords = "Meta Keywords",
                AddLanguageSlug = true,
                HomePageId = Guid.NewGuid(),
                ThemeId = Guid.NewGuid(),
                SiteLocalisations = new List<SiteLocalisation>
                {
                    new SiteLocalisation
                    {
                        LanguageId = Guid.NewGuid(),
                        Title = "Title",
                        MetaDescription = "Meta Description",
                        MetaKeywords = "Meta Keywords"
                    }
                }
            };
            _validatorMock = new Mock<IValidator<UpdateSiteDetails>>();
            _validatorMock.Setup(x => x.Validate(_command)).Returns(new ValidationResult());
            _site = new Site();
            _site.UpdateDetails(_command, _validatorMock.Object);
            _event = _site.Events.OfType<SiteDetailsUpdated>().SingleOrDefault();
        }

        [Test]
        public void Should_validate_command()
        {
            _validatorMock.Verify(x => x.Validate(_command));
        }

        [Test]
        public void Should_set_url()
        {
            Assert.AreEqual(_command.Url, _site.Url);
        }

        [Test]
        public void Should_set_head_title()
        {
            Assert.AreEqual(_command.Title, _site.Title);
        }

        [Test]
        public void Should_set_meta_description()
        {
            Assert.AreEqual(_command.MetaDescription, _site.MetaDescription);
        }

        [Test]
        public void Should_set_meta_keywords()
        {
            Assert.AreEqual(_command.MetaKeywords, _site.MetaKeywords);
        }

        [Test]
        public void Should_set_add_language_slug()
        {
            Assert.AreEqual(_command.AddLanguageSlug, _site.AddLanguageSlug);
        }

        [Test]
        public void Should_set_home_page_id()
        {
            Assert.AreEqual(_command.HomePageId, _site.HomePageId);
        }

        [Test]
        public void Should_set_theme_id()
        {
            Assert.AreEqual(_command.ThemeId, _site.ThemeId);
        }

        [Test]
        public void Should_set_localisation_language_id()
        {
            Assert.AreEqual(_command.SiteLocalisations[0].LanguageId, _site.SiteLocalisations.FirstOrDefault().LanguageId);
        }

        [Test]
        public void Should_set_localisation_head_title()
        {
            Assert.AreEqual(_command.SiteLocalisations[0].Title, _site.SiteLocalisations.FirstOrDefault().Title);
        }

        [Test]
        public void Should_set_localisation_meta_description()
        {
            Assert.AreEqual(_command.SiteLocalisations[0].MetaDescription, _site.SiteLocalisations.FirstOrDefault().MetaDescription);
        }

        [Test]
        public void Should_set_localisation_meta_keywords()
        {
            Assert.AreEqual(_command.SiteLocalisations[0].MetaKeywords, _site.SiteLocalisations.FirstOrDefault().MetaKeywords);
        }

        [Test]
        public void Should_add_site_details_updated_event()
        {
            Assert.IsNotNull(_event);
        }

        [Test]
        public void Should_set_id_in_site_details_updated_event()
        {
            Assert.AreEqual(_site.Id, _event.AggregateRootId);
        }

        [Test]
        public void Should_set_name_in_site_details_updated_event()
        {
            Assert.AreEqual(_site.Name, _event.Name);
        }

        [Test]
        public void Should_set_url_in_site_details_updated_event()
        {
            Assert.AreEqual(_site.Url, _event.Url);
        }

        [Test]
        public void Should_set_head_title_in_site_details_updated_event()
        {
            Assert.AreEqual(_site.Title, _event.Title);
        }

        [Test]
        public void Should_set_meta_description_in_site_details_updated_event()
        {
            Assert.AreEqual(_site.MetaDescription, _event.MetaDescription);
        }

        [Test]
        public void Should_set_meta_keywords_in_site_details_updated_event()
        {
            Assert.AreEqual(_site.MetaKeywords, _event.MetaKeywords);
        }

        [Test]
        public void Should_set_add_language_slug_in_site_details_updated_event()
        {
            Assert.AreEqual(_site.AddLanguageSlug, _event.AddLanguageSlug);
        }

        [Test]
        public void Should_set_home_page_id_in_site_details_updated_event()
        {
            Assert.AreEqual(_site.HomePageId, _event.HomePageId);
        }

        [Test]
        public void Should_set_theme_id_in_site_details_updated_event()
        {
            Assert.AreEqual(_site.ThemeId, _event.ThemeId);
        }

        [Test]
        public void Should_set_site_localisations_in_site_details_updated_event()
        {
            Assert.AreEqual(_site.SiteLocalisations, _event.SiteLocalisations);
        }
    }
}