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
    public class ReorderPageModulesTests
    {
        private ReorderPageModules command;
        private Page page;
        private PageModulesReordered @event;

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

            page = Page.CreateNew(createPageCommand, createPageValidatorMock.Object);

            page.AddModule(new PageModule(page.Id, Guid.NewGuid(), Guid.NewGuid(), "Title", "Header", 1));
            page.AddModule(new PageModule(page.Id, Guid.NewGuid(), Guid.NewGuid(), "Title", "Header", 2));
            page.AddModule(new PageModule(page.Id, Guid.NewGuid(), Guid.NewGuid(), "Title", "Content", 1));
            page.AddModule(new PageModule(page.Id, Guid.NewGuid(), Guid.NewGuid(), "Title", "Content", 2));
            page.AddModule(new PageModule(page.Id, Guid.NewGuid(), Guid.NewGuid(), "Title", "Footer", 1));
            page.AddModule(new PageModule(page.Id, Guid.NewGuid(), Guid.NewGuid(), "Title", "Footer", 2));

            command = new ReorderPageModules
            {
                SiteId = page.SiteId,
                PageId = page.Id,
                Zones = new List<ReorderPageModules.Zone>
                {
                    new ReorderPageModules.Zone
                    {
                        Name = "Header",
                        Modules = new List<Guid>
                        {
                            page.PageModules[0].ModuleId
                        }
                    },
                    new ReorderPageModules.Zone
                    {
                        Name = "Content",
                        Modules = new List<Guid>
                        {
                            page.PageModules[2].ModuleId,
                            page.PageModules[3].ModuleId,
                            page.PageModules[1].ModuleId
                        }
                    },
                    new ReorderPageModules.Zone
                    {
                        Name = "Footer",
                        Modules = new List<Guid>
                        {
                            page.PageModules[4].ModuleId,
                            page.PageModules[5].ModuleId
                        }
                    }
                }
            };

            var validatorMock = new Mock<IValidator<ReorderPageModules>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            page.ReorderPageModules(command, validatorMock.Object);

            @event = page.Events.OfType<PageModulesReordered>().SingleOrDefault();
        }

        [Test]
        public void Should_set_new_zone_for_moved_module()
        {
            Assert.AreEqual("Content", page.PageModules[1].Zone);
        }

        [Test]
        public void Should_set_new_sort_order_for_moved_module()
        {
            Assert.AreEqual(3, page.PageModules[1].SortOrder);
        }

        [Test]
        public void Should_add_page_modules_reordered_event()
        {
            Assert.IsNotNull(@event);
        }

        [Test]
        public void Should_set_id_in_page_modules_reordered_event()
        {
            Assert.AreEqual(page.Id, @event.AggregateRootId);
        }

        [Test]
        public void Should_set_site_id_in_page_modules_reordered_event()
        {
            Assert.AreEqual(page.SiteId, @event.SiteId);
        }

        [Test]
        public void Should_throw_exception_if_menu_item_does_not_exist()
        {
            command.Zones.FirstOrDefault().Modules.Add(Guid.NewGuid());
            var validatorMock = new Mock<IValidator<ReorderPageModules>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());
            Assert.Throws<Exception>(() => page.ReorderPageModules(command, validatorMock.Object));
        }
    }
}
