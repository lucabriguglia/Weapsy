using System;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Pages;
using Weapsy.Domain.Pages.Commands;
using Weapsy.Domain.Pages.Events;
using System.Collections.Generic;

namespace Weapsy.Domain.Tests.Pages
{
    [TestFixture]
    public class UpdatePageDetailsTests
    {
        private UpdatePageDetails _command;
        private Mock<IValidator<UpdatePageDetails>> _validatorMock;
        private Page _page;
        private PageDetailsUpdated _event;

        [SetUp]
        public void Setup()
        {
            var pageId = Guid.NewGuid();
            _command = new UpdatePageDetails
            {
                SiteId = Guid.NewGuid(),
                Id = pageId,
                Name = "Name",
                Url = "url",
                Title = "Title",
                MetaDescription = "Meta Description",
                MetaKeywords = "Meta Keywords",
                PageLocalisations = new List<PageLocalisation>
                {
                    new PageLocalisation
                    {
                        LanguageId = Guid.NewGuid(),
                        Url = "url",
                        Title = "Title",
                        MetaDescription = "Meta Description",
                        MetaKeywords = "Meta Keywords"
                    }
                },
                PagePermissions = new List<PagePermission>
                {
                    new PagePermission
                    {
                        RoleId = Guid.NewGuid(),
                        Type = PermissionType.View
                    }
                }
            };
            _validatorMock = new Mock<IValidator<UpdatePageDetails>>();
            _validatorMock.Setup(x => x.Validate(_command)).Returns(new ValidationResult());
            _page = new Page();
            _page.UpdateDetails(_command, _validatorMock.Object);
            _event = _page.Events.OfType<PageDetailsUpdated>().SingleOrDefault();
        }

        [Test]
        public void Should_validate_command()
        {
            _validatorMock.Verify(x => x.Validate(_command));
        }

        [Test]
        public void Should_set_name()
        {
            Assert.AreEqual(_command.Name, _page.Name);
        }

        [Test]
        public void Should_set_url()
        {
            Assert.AreEqual(_command.Url, _page.Url);
        }

        [Test]
        public void Should_set_head_title()
        {
            Assert.AreEqual(_command.Title, _page.Title);
        }

        [Test]
        public void Should_set_meta_description()
        {
            Assert.AreEqual(_command.MetaDescription, _page.MetaDescription);
        }

        [Test]
        public void Should_set_meta_keywords()
        {
            Assert.AreEqual(_command.MetaKeywords, _page.MetaKeywords);
        }

        [Test]
        public void Should_set_localisation_language_id()
        {
            Assert.AreEqual(_command.PageLocalisations[0].LanguageId, _page.PageLocalisations.FirstOrDefault().LanguageId);
        }

        [Test]
        public void Should_set_localisation_url()
        {
            Assert.AreEqual(_command.PageLocalisations[0].Url, _page.PageLocalisations.FirstOrDefault().Url);
        }

        [Test]
        public void Should_set_localisation_head_title()
        {
            Assert.AreEqual(_command.PageLocalisations[0].Title, _page.PageLocalisations.FirstOrDefault().Title);
        }

        [Test]
        public void Should_set_localisation_meta_description()
        {
            Assert.AreEqual(_command.PageLocalisations[0].MetaDescription, _page.PageLocalisations.FirstOrDefault().MetaDescription);
        }

        [Test]
        public void Should_set_localisation_meta_keywords()
        {
            Assert.AreEqual(_command.PageLocalisations[0].MetaKeywords, _page.PageLocalisations.FirstOrDefault().MetaKeywords);
        }

        [Test]
        public void Should_set_permission_role_id()
        {
            Assert.AreEqual(_command.PagePermissions[0].RoleId, _page.PagePermissions.FirstOrDefault().RoleId);
        }

        [Test]
        public void Should_set_permission_type()
        {
            Assert.AreEqual(_command.PagePermissions[0].Type, _page.PagePermissions.FirstOrDefault().Type);
        }

        [Test]
        public void Should_add_page_details_updated_event()
        {
            Assert.IsNotNull(_event);
        }

        [Test]
        public void Should_set_site_id_in_page_details_updated_event()
        {
            Assert.AreEqual(_page.SiteId, _event.SiteId);
        }

        [Test]
        public void Should_set_id_in_page_details_updated_event()
        {
            Assert.AreEqual(_page.Id, _event.AggregateRootId);
        }

        [Test]
        public void Should_set_name_in_page_details_updated_event()
        {
            Assert.AreEqual(_page.Name, _event.Name);
        }

        [Test]
        public void Should_set_url_in_page_details_updated_event()
        {
            Assert.AreEqual(_page.Url, _event.Url);
        }

        [Test]
        public void Should_set_head_title_in_page_details_updated_event()
        {
            Assert.AreEqual(_page.Title, _event.Title);
        }

        [Test]
        public void Should_set_meta_description_in_page_details_updated_event()
        {
            Assert.AreEqual(_page.MetaDescription, _event.MetaDescription);
        }

        [Test]
        public void Should_set_meta_keywords_in_page_details_updated_event()
        {
            Assert.AreEqual(_page.MetaKeywords, _event.MetaKeywords);
        }

        [Test]
        public void Should_set_page_localisations_in_page_details_updated_event()
        {
            Assert.AreEqual(_page.PageLocalisations, _event.PageLocalisations);
        }

        [Test]
        public void Should_set_page_permissions_in_page_created_event()
        {
            Assert.AreEqual(_page.PagePermissions, _event.PagePermissions);
        }

        [Test]
        public void Should_throw_exception_if_language_is_already_added_to_page_localisations()
        {
            var languageId = Guid.NewGuid();
            _command.PageLocalisations.Add(new PageLocalisation
            {
                LanguageId = languageId
            });
            _command.PageLocalisations.Add(new PageLocalisation
            {
                LanguageId = languageId
            });
            Assert.Throws<Exception>(() => _page.UpdateDetails(_command, _validatorMock.Object));
        }
    }
}