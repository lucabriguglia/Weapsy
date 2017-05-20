using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Modules;
using Weapsy.Domain.Modules.Commands;
using Weapsy.Domain.Modules.Events;
using Weapsy.Domain.Pages;
using Weapsy.Domain.Pages.Commands;
using Weapsy.Domain.Pages.Events;
using Weapsy.Domain.Pages.Handlers;
using Weapsy.Framework.Events;
using Weapsy.Tests.Factories;

namespace Weapsy.Domain.Tests.Pages.Handlers
{
    [TestFixture]
    public class RemoveModuleHandlerTests
    {
        [Test]
        public void Should_throw_exception_when_page_is_not_found()
        {
            var command = new RemoveModule
            {
                SiteId = Guid.NewGuid(),
                PageId = Guid.NewGuid(),
                ModuleId = Guid.NewGuid()
            };

            var moduleRepositoryMock = new Mock<IModuleRepository>();
            moduleRepositoryMock.Setup(x => x.Create(It.IsAny<Module>()));

            var pageRepositoryMock = new Mock<IPageRepository>();
            pageRepositoryMock.Setup(x => x.GetById(command.SiteId, command.PageId)).Returns((Page)null);

            var deleteModuleValidatorMock = new Mock<IValidator<DeleteModule>>();
            deleteModuleValidatorMock.Setup(x => x.Validate(It.IsAny<DeleteModule>())).Returns(new ValidationResult());

            var removePageModuleValidatorMock = new Mock<IValidator<RemovePageModule>>();
            removePageModuleValidatorMock.Setup(x => x.Validate(It.IsAny<RemovePageModule>())).Returns(new ValidationResult());

            var removeModuleHandler = new RemoveModuleHandler(moduleRepositoryMock.Object, pageRepositoryMock.Object, deleteModuleValidatorMock.Object, removePageModuleValidatorMock.Object);

            Assert.Throws<Exception>(() => removeModuleHandler.Handle(command));
        }

        [Test]
        public void Should_remove_module_from_page()
        {
            var command = new RemoveModule
            {
                SiteId = Guid.NewGuid(),
                PageId = Guid.NewGuid(),
                ModuleId = Guid.NewGuid()
            };

            var page = PageFactory.Page(command.SiteId, command.PageId, "My Page", Guid.NewGuid(), command.ModuleId);

            var moduleRepositoryMock = new Mock<IModuleRepository>();
            moduleRepositoryMock.Setup(x => x.GetById(command.SiteId, command.ModuleId)).Returns(new Module());

            var pageRepositoryMock = new Mock<IPageRepository>();
            pageRepositoryMock.Setup(x => x.GetById(command.SiteId, command.PageId)).Returns(page);
            pageRepositoryMock.Setup(x => x.Update(It.IsAny<Page>()));

            var deleteModuleValidatorMock = new Mock<IValidator<DeleteModule>>();
            deleteModuleValidatorMock.Setup(x => x.Validate(It.IsAny<DeleteModule>())).Returns(new ValidationResult());

            var removePageModuleValidatorMock = new Mock<IValidator<RemovePageModule>>();
            removePageModuleValidatorMock.Setup(x => x.Validate(It.IsAny<RemovePageModule>())).Returns(new ValidationResult());

            var removeModuleHandler = new RemoveModuleHandler(moduleRepositoryMock.Object, 
                pageRepositoryMock.Object, 
                deleteModuleValidatorMock.Object, 
                removePageModuleValidatorMock.Object);

            removeModuleHandler.Handle(command);

            var @event = page.Events.OfType<PageModuleRemoved>().SingleOrDefault();

            Assert.IsNotNull(@event);
        }

        [Test]       
        public void Should_update_page()
        {
            var command = new RemoveModule
            {
                SiteId = Guid.NewGuid(),
                PageId = Guid.NewGuid(),
                ModuleId = Guid.NewGuid()
            };

            var page = PageFactory.Page(command.SiteId, command.PageId, "My Page", Guid.NewGuid(), command.ModuleId);

            var moduleRepositoryMock = new Mock<IModuleRepository>();
            moduleRepositoryMock.Setup(x => x.GetById(command.SiteId, command.ModuleId)).Returns(new Module());

            var pageRepositoryMock = new Mock<IPageRepository>();
            pageRepositoryMock.Setup(x => x.GetById(command.SiteId, command.PageId)).Returns(page);
            pageRepositoryMock.Setup(x => x.Update(It.IsAny<Page>()));

            var deleteModuleValidatorMock = new Mock<IValidator<DeleteModule>>();
            deleteModuleValidatorMock.Setup(x => x.Validate(It.IsAny<DeleteModule>())).Returns(new ValidationResult());

            var removePageModuleValidatorMock = new Mock<IValidator<RemovePageModule>>();
            removePageModuleValidatorMock.Setup(x => x.Validate(It.IsAny<RemovePageModule>())).Returns(new ValidationResult());

            var removeModuleHandler = new RemoveModuleHandler(moduleRepositoryMock.Object, 
                pageRepositoryMock.Object, 
                deleteModuleValidatorMock.Object, 
                removePageModuleValidatorMock.Object);

            removeModuleHandler.Handle(command);

            pageRepositoryMock.Verify(x => x.Update(It.IsAny<Page>()));
        }

        [Test]
        public void Should_not_delete_module_if_used_in_other_pages()
        {
            var siteId = Guid.NewGuid();
            var pageId = Guid.NewGuid();
            var pageModuleId = Guid.NewGuid();
            var moduleId = Guid.NewGuid();

            var page = PageFactory.Page(siteId, pageId, "Name", pageModuleId, moduleId);

            var command = new RemoveModule
            {
                SiteId = siteId,
                PageId = pageId,
                ModuleId = moduleId
            };

            var moduleRepositoryMock = new Mock<IModuleRepository>();
            moduleRepositoryMock.Setup(x => x.GetById(command.SiteId, command.ModuleId)).Returns(new Module());
            moduleRepositoryMock.Setup(x => x.GetCountByModuleId(command.ModuleId)).Returns(2);
            moduleRepositoryMock.Setup(x => x.Update(It.IsAny<Module>()));

            var pageRepositoryMock = new Mock<IPageRepository>();
            pageRepositoryMock.Setup(x => x.GetById(command.SiteId, command.PageId)).Returns(page);

            var deleteModuleValidatorMock = new Mock<IValidator<DeleteModule>>();
            deleteModuleValidatorMock.Setup(x => x.Validate(It.IsAny<DeleteModule>())).Returns(new ValidationResult());

            var removePageModuleValidatorMock = new Mock<IValidator<RemovePageModule>>();
            removePageModuleValidatorMock.Setup(x => x.Validate(It.IsAny<RemovePageModule>())).Returns(new ValidationResult());

            var removeModuleHandler = new RemoveModuleHandler(moduleRepositoryMock.Object, pageRepositoryMock.Object, deleteModuleValidatorMock.Object, removePageModuleValidatorMock.Object);

            removeModuleHandler.Handle(command);

            moduleRepositoryMock.Verify(x => x.Update(It.IsAny<Module>()), Times.Never);
        }

        [Test]
        public void Should_delete_module_if_not_used_in_other_pages()
        {
            var command = new RemoveModule
            {
                SiteId = Guid.NewGuid(),
                PageId = Guid.NewGuid(),
                ModuleId = Guid.NewGuid()
            };

            var deleteModuleValidatorMock = new Mock<IValidator<DeleteModule>>();
            deleteModuleValidatorMock.Setup(x => x.Validate(It.IsAny<DeleteModule>())).Returns(new ValidationResult());

            var removePageModuleValidatorMock = new Mock<IValidator<RemovePageModule>>();
            removePageModuleValidatorMock.Setup(x => x.Validate(It.IsAny<RemovePageModule>())).Returns(new ValidationResult());

            var moduleRepositoryMock = new Mock<IModuleRepository>();
            moduleRepositoryMock.Setup(x => x.GetCountByModuleId(command.ModuleId)).Returns(1);
            moduleRepositoryMock.Setup(x => x.GetById(command.SiteId, command.ModuleId)).Returns(new Module());
            moduleRepositoryMock.Setup(x => x.Update(It.IsAny<Module>()));

            var page = PageFactory.Page(command.SiteId, command.PageId, "My Page", Guid.NewGuid(), command.ModuleId);

            var pageRepositoryMock = new Mock<IPageRepository>();
            pageRepositoryMock.Setup(x => x.GetById(command.SiteId, command.PageId)).Returns(page);

            var removeModuleHandler = new RemoveModuleHandler(moduleRepositoryMock.Object, pageRepositoryMock.Object, deleteModuleValidatorMock.Object, removePageModuleValidatorMock.Object);

            removeModuleHandler.Handle(command);

            moduleRepositoryMock.Verify(x => x.Update(It.IsAny<Module>()));
        }

        [Test]       
        public void Should_update_module_if_not_used_in_other_pages()
        {
            var siteId = Guid.NewGuid();
            var pageId = Guid.NewGuid();
            var moduleId = Guid.NewGuid();

            var addPageModuleCommand = new AddPageModule
            {
                SiteId = siteId,
                PageId = pageId,
                ModuleId = moduleId,
                PageModuleId = Guid.NewGuid(),
                Zone = "Zone",
                SortOrder = 1,
                Title = "Title"
            };

            var addPageModuleValidatoMock = new Mock<IValidator<AddPageModule>>();
            addPageModuleValidatoMock.Setup(x => x.Validate(addPageModuleCommand)).Returns(new ValidationResult());

            var page = PageFactory.Page(siteId, pageId, "My Page", Guid.NewGuid(), addPageModuleCommand.ModuleId);

            var command = new RemoveModule
            {
                SiteId = siteId,
                PageId = pageId,
                ModuleId = moduleId
            };

            var moduleRepositoryMock = new Mock<IModuleRepository>();
            moduleRepositoryMock.Setup(x => x.GetCountByModuleId(command.ModuleId)).Returns(1);
            moduleRepositoryMock.Setup(x => x.GetById(command.SiteId, command.ModuleId)).Returns(new Module());
            moduleRepositoryMock.Setup(x => x.Update(It.IsAny<Module>()));

            var pageRepositoryMock = new Mock<IPageRepository>();
            pageRepositoryMock.Setup(x => x.GetById(command.SiteId, command.PageId)).Returns(page);

            var deleteModuleValidatorMock = new Mock<IValidator<DeleteModule>>();
            deleteModuleValidatorMock.Setup(x => x.Validate(It.IsAny<DeleteModule>())).Returns(new ValidationResult());

            var removePageModuleValidatorMock = new Mock<IValidator<RemovePageModule>>();
            removePageModuleValidatorMock.Setup(x => x.Validate(It.IsAny<RemovePageModule>())).Returns(new ValidationResult());

            var removeModuleHandler = new RemoveModuleHandler(moduleRepositoryMock.Object, 
                pageRepositoryMock.Object, 
                deleteModuleValidatorMock.Object, 
                removePageModuleValidatorMock.Object);

            removeModuleHandler.Handle(command);

            moduleRepositoryMock.Verify(x => x.Update(It.IsAny<Module>()));
        }

        [Test]
        public void Should_return_one_event_if_module_is_used_in_other_pages()
        {
            var siteId = Guid.NewGuid();
            var pageId = Guid.NewGuid();
            var moduleId = Guid.NewGuid();

            var addPageModuleCommand = new AddPageModule
            {
                SiteId = siteId,
                PageId = pageId,
                ModuleId = moduleId,
                PageModuleId = Guid.NewGuid(),
                Zone = "Zone",
                SortOrder = 1,
                Title = "Title"
            };

            var addPageModuleValidatoMock = new Mock<IValidator<AddPageModule>>();
            addPageModuleValidatoMock.Setup(x => x.Validate(addPageModuleCommand)).Returns(new ValidationResult());

            var page = new Page();

            page.AddModule(addPageModuleCommand, addPageModuleValidatoMock.Object);

            page.Events.Clear();

            var command = new RemoveModule
            {
                SiteId = siteId,
                PageId = pageId,
                ModuleId = moduleId
            };

            var moduleRepositoryMock = new Mock<IModuleRepository>();
            moduleRepositoryMock.Setup(x => x.GetCountByModuleId(command.ModuleId)).Returns(2);
            moduleRepositoryMock.Setup(x => x.GetById(command.SiteId, command.ModuleId)).Returns(new Module());

            var pageRepositoryMock = new Mock<IPageRepository>();
            pageRepositoryMock.Setup(x => x.GetById(command.SiteId, command.PageId)).Returns(page);

            var deleteModuleValidatorMock = new Mock<IValidator<DeleteModule>>();
            deleteModuleValidatorMock.Setup(x => x.Validate(It.IsAny<DeleteModule>())).Returns(new ValidationResult());

            var removePageModuleValidatorMock = new Mock<IValidator<RemovePageModule>>();
            removePageModuleValidatorMock.Setup(x => x.Validate(It.IsAny<RemovePageModule>())).Returns(new ValidationResult());

            var removeModuleHandler = new RemoveModuleHandler(moduleRepositoryMock.Object, pageRepositoryMock.Object, deleteModuleValidatorMock.Object, removePageModuleValidatorMock.Object);

            var events = removeModuleHandler.Handle(command);

            Assert.AreEqual(typeof(PageModuleRemoved), events.FirstOrDefault().GetType());
        }

        [Test]
        public void Should_return_two_events_if_module_is_not_used_in_other_pages()
        {
            var siteId = Guid.NewGuid();
            var pageId = Guid.NewGuid();
            var moduleId = Guid.NewGuid();

            var addPageModuleCommand = new AddPageModule
            {
                SiteId = siteId,
                PageId = pageId,
                ModuleId = moduleId,
                PageModuleId = Guid.NewGuid(),
                Zone = "Zone",
                SortOrder = 1,
                Title = "Title"
            };

            var addPageModuleValidatoMock = new Mock<IValidator<AddPageModule>>();
            addPageModuleValidatoMock.Setup(x => x.Validate(addPageModuleCommand)).Returns(new ValidationResult());

            var page = new Page();

            page.AddModule(addPageModuleCommand, addPageModuleValidatoMock.Object);

            page.Events.Clear();

            var command = new RemoveModule
            {
                SiteId = siteId,
                PageId = pageId,
                ModuleId = moduleId
            };

            var moduleRepositoryMock = new Mock<IModuleRepository>();
            moduleRepositoryMock.Setup(x => x.GetCountByModuleId(command.ModuleId)).Returns(1);
            moduleRepositoryMock.Setup(x => x.GetById(command.SiteId, command.ModuleId)).Returns(new Module());

            var pageRepositoryMock = new Mock<IPageRepository>();
            pageRepositoryMock.Setup(x => x.GetById(command.SiteId, command.PageId)).Returns(page);

            var deleteModuleValidatorMock = new Mock<IValidator<DeleteModule>>();
            deleteModuleValidatorMock.Setup(x => x.Validate(It.IsAny<DeleteModule>())).Returns(new ValidationResult());

            var removePageModuleValidatorMock = new Mock<IValidator<RemovePageModule>>();
            removePageModuleValidatorMock.Setup(x => x.Validate(It.IsAny<RemovePageModule>())).Returns(new ValidationResult());

            var removeModuleHandler = new RemoveModuleHandler(moduleRepositoryMock.Object, pageRepositoryMock.Object, deleteModuleValidatorMock.Object, removePageModuleValidatorMock.Object);

            var events = removeModuleHandler.Handle(command);

            var enumerable = events as IList<IEvent> ?? events.ToList();
            Assert.AreEqual(typeof(PageModuleRemoved), enumerable.FirstOrDefault().GetType());
            Assert.AreEqual(typeof(ModuleDeleted), enumerable.Skip(1).FirstOrDefault().GetType());
        }
    }
}
