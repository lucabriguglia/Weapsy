using System;
using System.Linq;
using NUnit.Framework;
using Weapsy.Domain.Pages;
using Weapsy.Domain.Pages.Commands;
using Weapsy.Domain.Pages.Events;
using System.Collections.Generic;
using Weapsy.Domain.Roles.DefaultRoles;
using Weapsy.Tests.Factories;

namespace Weapsy.Domain.Tests.Pages
{
    [TestFixture]
    public class SetPagePermissionsTests
    {
        private SetPagePermissions _command;
        private Page _page;
        private PagePermissionsSet _event;

        [SetUp]
        public void Setup()
        {
            var siteId = Guid.NewGuid();
            var pageId = Guid.NewGuid();
            var pageName = "Name";

            _page = PageFactory.Page(siteId, pageId, pageName);

            _command = new SetPagePermissions
            {
                SiteId = Guid.NewGuid(),
                Id = pageId,
                PagePermissions = new List<PagePermission>
                {
                    new PagePermission
                    {
                        PageId = pageId,
                        RoleId = Everyone.Id,
                        Type = PermissionType.View
                    }
                }
            };

            _page.SetPagePermissions(_command);

            _event = _page.Events.OfType<PagePermissionsSet>().SingleOrDefault();
        }

        [Test]
        public void Should_set_page_permissions()
        {
            Assert.AreEqual(_command.PagePermissions.FirstOrDefault().RoleId, _page.PagePermissions.FirstOrDefault().RoleId);
        }

        [Test]
        public void Should_add_page_permissions_set_event()
        {
            Assert.IsNotNull(_event);
        }

        [Test]
        public void Should_set_site_id_in_page_permissions_set_event()
        {
            Assert.AreEqual(_page.SiteId, _event.SiteId);
        }

        [Test]
        public void Should_set_id_in_page_permissions_set_event()
        {
            Assert.AreEqual(_page.Id, _event.AggregateRootId);
        }

        [Test]
        public void Should_set_page_permissions_in_page_permissions_set_event()
        {
            Assert.AreEqual(_page.PagePermissions.FirstOrDefault().RoleId, _event.PagePermissions.FirstOrDefault().RoleId);
        }
    }
}