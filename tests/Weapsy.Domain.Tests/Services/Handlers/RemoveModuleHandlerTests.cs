using System;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Pages;
using Weapsy.Domain.Services.Modules.Commands;
using Weapsy.Domain.Services.Modules.Handlers;
using Weapsy.Domain.Modules;
using Weapsy.Domain.Modules.Events;
using Weapsy.Domain.Pages.Events;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using Weapsy.Domain.Pages.Commands;
using Weapsy.Domain.Modules.Commands;
using Weapsy.Domain.Modules.Rules;

namespace Weapsy.Domain.Tests.Services.Handlers
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
        [Ignore("Non-virtual Member")]
        public void Should_remove_module_from_page()
        {
            var command = new RemoveModule
            {
                SiteId = Guid.NewGuid(),
                PageId = Guid.NewGuid(),
                ModuleId = Guid.NewGuid()
            };

            var removePageModuleCommand = new RemovePageModule
            {
                SiteId = command.SiteId,
                PageId = command.PageId,
                ModuleId = command.ModuleId,
            };

            var removePageModuleValidatorMock = new Mock<IValidator<RemovePageModule>>();
            removePageModuleValidatorMock.Setup(x => x.Validate(removePageModuleValidatorMock)).Returns(new ValidationResult());

            var pageMock = new Mock<Page>();
            pageMock.Setup(x => x.RemoveModule(removePageModuleCommand, removePageModuleValidatorMock.Object));

            var moduleRepositoryMock = new Mock<IModuleRepository>();
            var pageRepositoryMock = new Mock<IPageRepository>();
            pageRepositoryMock.Setup(x => x.GetById(command.SiteId, command.PageId)).Returns(pageMock.Object);
            pageRepositoryMock.Setup(x => x.Update(It.IsAny<Page>()));

            var deleteModuleValidatorMock = new Mock<IValidator<DeleteModule>>();
            deleteModuleValidatorMock.Setup(x => x.Validate(It.IsAny<DeleteModule>())).Returns(new ValidationResult());

            var removeModuleHandler = new RemoveModuleHandler(moduleRepositoryMock.Object, pageRepositoryMock.Object, deleteModuleValidatorMock.Object, removePageModuleValidatorMock.Object);

            removeModuleHandler.Handle(command);

            pageMock.Verify(x => x.RemoveModule(removePageModuleCommand, removePageModuleValidatorMock.Object));
        }

        [Ignore("Non-virtual Member")]
        [Test]       
        public void Should_update_page()
        {
            var command = new RemoveModule
            {
                SiteId = Guid.NewGuid(),
                PageId = Guid.NewGuid(),
                ModuleId = Guid.NewGuid()
            };

            var removePageModuleCommand = new RemovePageModule
            {
                SiteId = command.SiteId,
                PageId = command.PageId,
                ModuleId = command.ModuleId,
            };

            var removePageModuleValidatorMock = new Mock<IValidator<RemovePageModule>>();
            removePageModuleValidatorMock.Setup(x => x.Validate(removePageModuleValidatorMock)).Returns(new ValidationResult());

            var pageMock = new Mock<Page>();
            pageMock.Setup(x => x.RemoveModule(removePageModuleCommand, removePageModuleValidatorMock.Object));

            var moduleRepositoryMock = new Mock<IModuleRepository>();

            var pageRepositoryMock = new Mock<IPageRepository>();
            pageRepositoryMock.Setup(x => x.GetById(command.SiteId, command.PageId)).Returns(pageMock.Object);
            pageRepositoryMock.Setup(x => x.Update(It.IsAny<Page>()));

            var deleteModuleValidatorMock = new Mock<IValidator<DeleteModule>>();
            deleteModuleValidatorMock.Setup(x => x.Validate(It.IsAny<DeleteModule>())).Returns(new ValidationResult());

            var removeModuleHandler = new RemoveModuleHandler(moduleRepositoryMock.Object, pageRepositoryMock.Object, deleteModuleValidatorMock.Object, removePageModuleValidatorMock.Object);

            removeModuleHandler.Handle(command);

            pageRepositoryMock.Verify(x => x.Update(It.IsAny<Page>()));
        }

        [Test]
        public void Should_not_delete_module_if_used_in_other_pages()
        {
            var siteId = Guid.NewGuid();
            var pageId = Guid.NewGuid();
            var moduleId = Guid.NewGuid();

            var addPageModuleCommand = new AddPageModule
            {
                SiteId = siteId,
                PageId = pageId,
                ModuleId = moduleId,
                Id = Guid.NewGuid(),
                Zone = "Zone",
                SortOrder = 1,
                Title = "Title"
            };

            var addPageModuleValidatoMock = new Mock<IValidator<AddPageModule>>();
            addPageModuleValidatoMock.Setup(x => x.Validate(addPageModuleCommand)).Returns(new ValidationResult());

            var page = new Page();

            page.AddModule(addPageModuleCommand, addPageModuleValidatoMock.Object);

            var command = new RemoveModule
            {
                SiteId = siteId,
                PageId = pageId,
                ModuleId = moduleId
            };

            var moduleRepositoryMock = new Mock<IModuleRepository>();
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
        [Ignore("Non-virtual Member")]
        public void Should_delete_module_if_not_used_in_other_pages()
        {
            var command = new RemoveModule
            {
                SiteId = Guid.NewGuid(),
                PageId = Guid.NewGuid(),
                ModuleId = Guid.NewGuid()
            };

            var deleteModuleCommand = new DeleteModule
            {
                SiteId = command.SiteId,
                Id = command.ModuleId
            };

            var deleteModuleValidatorMock = new Mock<IValidator<DeleteModule>>();
            deleteModuleValidatorMock.Setup(x => x.Validate(It.IsAny<DeleteModule>())).Returns(new ValidationResult());

            var removePageModuleValidatorMock = new Mock<IValidator<RemovePageModule>>();
            removePageModuleValidatorMock.Setup(x => x.Validate(It.IsAny<RemovePageModule>())).Returns(new ValidationResult());

            var moduleMock = new Mock<Module>();
            moduleMock.Setup(x => x.Delete(deleteModuleCommand, deleteModuleValidatorMock.Object));

            var moduleRepositoryMock = new Mock<IModuleRepository>();
            moduleRepositoryMock.Setup(x => x.GetCountByModuleId(command.ModuleId)).Returns(1);
            moduleRepositoryMock.Setup(x => x.GetById(command.ModuleId)).Returns(moduleMock.Object);

            var pageRepositoryMock = new Mock<IPageRepository>();
            pageRepositoryMock.Setup(x => x.GetById(command.SiteId, command.PageId)).Returns(new Page());

            var removeModuleHandler = new RemoveModuleHandler(moduleRepositoryMock.Object, pageRepositoryMock.Object, deleteModuleValidatorMock.Object, removePageModuleValidatorMock.Object);

            removeModuleHandler.Handle(command);

            moduleMock.Verify(x => x.Delete(deleteModuleCommand, deleteModuleValidatorMock.Object));
        }

        [Ignore("Non-virtual Member")]
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
                Id = Guid.NewGuid(),
                Zone = "Zone",
                SortOrder = 1,
                Title = "Title"
            };

            var addPageModuleValidatoMock = new Mock<IValidator<AddPageModule>>();
            addPageModuleValidatoMock.Setup(x => x.Validate(addPageModuleCommand)).Returns(new ValidationResult());

            var page = new Page();

            page.AddModule(addPageModuleCommand, addPageModuleValidatoMock.Object);

            var command = new RemoveModule
            {
                SiteId = siteId,
                PageId = pageId,
                ModuleId = moduleId
            };

            var deleteModuleCommand = new DeleteModule
            {
                SiteId = command.SiteId,
                Id = command.ModuleId
            };

            var deleteModuleValidatorMock = new Mock<IValidator<DeleteModule>>();
            deleteModuleValidatorMock.Setup(x => x.Validate(It.IsAny<DeleteModule>())).Returns(new ValidationResult());

            var removePageModuleValidatorMock = new Mock<IValidator<RemovePageModule>>();
            removePageModuleValidatorMock.Setup(x => x.Validate(It.IsAny<RemovePageModule>())).Returns(new ValidationResult());

            var moduleMock = new Mock<Module>();
            moduleMock.Setup(x => x.Delete(deleteModuleCommand, deleteModuleValidatorMock.Object));

            var moduleRepositoryMock = new Mock<IModuleRepository>();
            moduleRepositoryMock.Setup(x => x.GetCountByModuleId(command.ModuleId)).Returns(1);
            moduleRepositoryMock.Setup(x => x.GetById(command.ModuleId)).Returns(moduleMock.Object);
            moduleRepositoryMock.Setup(x => x.Update(It.IsAny<Module>()));

            var pageRepositoryMock = new Mock<IPageRepository>();
            pageRepositoryMock.Setup(x => x.GetById(command.SiteId, command.PageId)).Returns(new Page());

            var removeModuleHandler = new RemoveModuleHandler(moduleRepositoryMock.Object, pageRepositoryMock.Object, deleteModuleValidatorMock.Object, removePageModuleValidatorMock.Object);

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
                Id = Guid.NewGuid(),
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

            Assert.AreEqual(1, events.Count);
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
                Id = Guid.NewGuid(),
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

            Assert.AreEqual(2, events.Count);
            Assert.AreEqual(typeof(PageModuleRemoved), events.FirstOrDefault().GetType());
            Assert.AreEqual(typeof(ModuleDeleted), events.Skip(1).FirstOrDefault().GetType());
        }
    }
}
