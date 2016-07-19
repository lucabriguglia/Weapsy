using System;
using System.Linq;
using NUnit.Framework;
using Weapsy.Domain.Model.Pages;
using Weapsy.Domain.Model.Pages.Commands;
using Weapsy.Domain.Model.Pages.Events;
using System.Collections.Generic;
using Weapsy.Tests.Factories;

namespace Weapsy.Domain.Tests.Pages
{
    [TestFixture]
    public class SetPageModulePermissionsTests
    {
        private SetPageModulePermissions _command;
        private Page _page;
        private PageModule _pageModule;
        private PageModulePermissionsSet _event;

        [SetUp]
        public void Setup()
        {
            var siteId = Guid.NewGuid();
            var pageId = Guid.NewGuid();
            var pageModuleId = Guid.NewGuid();
            var pageName = "Name";

            _page = PageFactory.Page(siteId, pageId, pageName, pageModuleId);
            _pageModule = _page.PageModules.FirstOrDefault(x => x.Id == pageModuleId);

            _command = new SetPageModulePermissions
            {
                SiteId = Guid.NewGuid(),
                Id = pageId,
                PageModulePermissions = new List<PageModulePermission>
                {
                    new PageModulePermission
                    {
                        PageModuleId = pageModuleId,
                        RoleId = "1",
                        Type = PermissionType.View
                    }
                }
            };

            _page.SetModulePermissions(_command);

            _event = _page.Events.OfType<PageModulePermissionsSet>().SingleOrDefault();
        }

        [Test]
        public void Should_set_module_permissions()
        {
            Assert.AreEqual(_command.PageModulePermissions, _pageModule.PageModulePermissions);
        }

        [Test]
        public void Should_add_page_module_permissions_set_event()
        {
            Assert.IsNotNull(_event);
        }

        [Test]
        public void Should_set_site_id_in_page_module_permissions_set_event()
        {
            Assert.AreEqual(_page.SiteId, _event.SiteId);
        }

        [Test]
        public void Should_set_id_in_page_module_permissions_set_event()
        {
            Assert.AreEqual(_page.Id, _event.AggregateRootId);
        }

        [Test]
        public void Should_set_page_permissions_in_page_module_permissions_set_event()
        {
            Assert.AreEqual(_pageModule.PageModulePermissions, _event.PageModulePermissions);
        }
    }
}