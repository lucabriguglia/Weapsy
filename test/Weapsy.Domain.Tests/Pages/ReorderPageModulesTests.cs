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
    public class ReorderPageModulesTests
    {
        private ReorderPageModules _command;
        private Page _page;
        private PageModulesReordered _event;

        [SetUp]
        public void Setup()
        {
            var createPageCommand = new CreatePage
            {
                SiteId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Name = "Name",
                Url = "url"
            };

            var createPageValidatorMock = new Mock<IValidator<CreatePage>>();
            createPageValidatorMock.Setup(x => x.Validate(createPageCommand)).Returns(new ValidationResult());

            _page = Page.CreateNew(createPageCommand, createPageValidatorMock.Object);

            _page.AddModule(new PageModule(_page.Id, Guid.NewGuid(), Guid.NewGuid(), "Title", "Header", 1, new List<PageModulePermission>()));
            _page.AddModule(new PageModule(_page.Id, Guid.NewGuid(), Guid.NewGuid(), "Title", "Header", 2, new List<PageModulePermission>()));
            _page.AddModule(new PageModule(_page.Id, Guid.NewGuid(), Guid.NewGuid(), "Title", "Content", 1, new List<PageModulePermission>()));
            _page.AddModule(new PageModule(_page.Id, Guid.NewGuid(), Guid.NewGuid(), "Title", "Content", 2, new List<PageModulePermission>()));
            _page.AddModule(new PageModule(_page.Id, Guid.NewGuid(), Guid.NewGuid(), "Title", "Footer", 1, new List<PageModulePermission>()));
            _page.AddModule(new PageModule(_page.Id, Guid.NewGuid(), Guid.NewGuid(), "Title", "Footer", 2, new List<PageModulePermission>()));

            _command = new ReorderPageModules
            {
                SiteId = _page.SiteId,
                PageId = _page.Id,
                Zones = new List<ReorderPageModules.Zone>
                {
                    new ReorderPageModules.Zone
                    {
                        Name = "Header",
                        Modules = new List<Guid>
                        {
                            _page.PageModules.FirstOrDefault().ModuleId
                        }
                    },
                    new ReorderPageModules.Zone
                    {
                        Name = "Content",
                        Modules = new List<Guid>
                        {
                            _page.PageModules.Skip(2).FirstOrDefault().ModuleId,
                            _page.PageModules.Skip(3).FirstOrDefault().ModuleId,
                            _page.PageModules.Skip(1).FirstOrDefault().ModuleId
                        }
                    },
                    new ReorderPageModules.Zone
                    {
                        Name = "Footer",
                        Modules = new List<Guid>
                        {
                            _page.PageModules.Skip(4).FirstOrDefault().ModuleId,
                            _page.PageModules.Skip(5).FirstOrDefault().ModuleId
                        }
                    }
                }
            };

            var validatorMock = new Mock<IValidator<ReorderPageModules>>();
            validatorMock.Setup(x => x.Validate(_command)).Returns(new ValidationResult());

            _page.ReorderPageModules(_command, validatorMock.Object);

            _event = _page.Events.OfType<PageModulesReordered>().SingleOrDefault();
        }

        [Test]
        public void Should_set_new_zone_for_moved_module()
        {
            Assert.AreEqual("Content", _page.PageModules.Skip(1).FirstOrDefault().Zone);
        }

        [Test]
        public void Should_set_new_sort_order_for_moved_module()
        {
            Assert.AreEqual(3, _page.PageModules.Skip(1).FirstOrDefault().SortOrder);
        }

        [Test]
        public void Should_add_page_modules_reordered_event()
        {
            Assert.IsNotNull(_event);
        }

        [Test]
        public void Should_set_id_in_page_modules_reordered_event()
        {
            Assert.AreEqual(_page.Id, _event.AggregateRootId);
        }

        [Test]
        public void Should_set_site_id_in_page_modules_reordered_event()
        {
            Assert.AreEqual(_page.SiteId, _event.SiteId);
        }

        [Test]
        public void Should_throw_exception_if_menu_item_does_not_exist()
        {
            _command.Zones.FirstOrDefault().Modules.Add(Guid.NewGuid());
            var validatorMock = new Mock<IValidator<ReorderPageModules>>();
            validatorMock.Setup(x => x.Validate(_command)).Returns(new ValidationResult());
            Assert.Throws<Exception>(() => _page.ReorderPageModules(_command, validatorMock.Object));
        }
    }
}
