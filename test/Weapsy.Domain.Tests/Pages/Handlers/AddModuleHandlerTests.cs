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
    public class AddModuleHandlerTests
    {
        [Test]
        public void Should_create_module()
        {
            var command = new AddModule
            {
                SiteId = Guid.NewGuid(),
                PageId = Guid.NewGuid(),
                ModuleTypeId = Guid.NewGuid(),
                Zone = "Zone",
                SortOrder = 1,
                Title = "Title"
            };

            var moduleRepositoryMock = new Mock<IModuleRepository>();
            moduleRepositoryMock.Setup(x => x.Create(It.IsAny<Module>()));
            var pageRepositoryMock = new Mock<IPageRepository>();
            pageRepositoryMock.Setup(x => x.GetById(command.SiteId, command.PageId)).Returns(new Page());
            var createModuleValidatorMock = new Mock<IValidator<CreateModule>>();
            createModuleValidatorMock.Setup(x => x.Validate(It.IsAny<CreateModule>())).Returns(new ValidationResult());
            var addPageModuleValidatorMock = new Mock<IValidator<AddPageModule>>();
            addPageModuleValidatorMock.Setup(x => x.Validate(It.IsAny<AddPageModule>())).Returns(new ValidationResult());

            var addModuleHandler = new AddModuleHandler(moduleRepositoryMock.Object,
                pageRepositoryMock.Object,
                createModuleValidatorMock.Object,
                addPageModuleValidatorMock.Object);

            addModuleHandler.Handle(command);

            moduleRepositoryMock.Verify(x => x.Create(It.IsAny<Module>()));
        }

        [Test]
        public void Should_throw_exception_when_page_is_not_found()
        {
            var command = new AddModule
            {
                SiteId = Guid.NewGuid(),
                PageId = Guid.NewGuid(),
                ModuleTypeId = Guid.NewGuid(),
                Zone = "Zone",
                SortOrder = 1,
                Title = "Title"
            };

            var moduleRepositoryMock = new Mock<IModuleRepository>();
            var pageRepositoryMock = new Mock<IPageRepository>();
            pageRepositoryMock.Setup(x => x.GetById(command.SiteId, command.PageId)).Returns((Page)null);
            var createModuleValidatorMock = new Mock<IValidator<CreateModule>>();
            createModuleValidatorMock.Setup(x => x.Validate(It.IsAny<CreateModule>())).Returns(new ValidationResult());
            var addPageModuleValidatorMock = new Mock<IValidator<AddPageModule>>();
            addPageModuleValidatorMock.Setup(x => x.Validate(It.IsAny<AddPageModule>())).Returns(new ValidationResult());

            var addModuleHandler = new AddModuleHandler(moduleRepositoryMock.Object, 
                pageRepositoryMock.Object, 
                createModuleValidatorMock.Object, 
                addPageModuleValidatorMock.Object);

            Assert.Throws<Exception>(() => addModuleHandler.Handle(command));
        }

        [Test]
        public void Should_add_module_to_page()
        {
            var command = new AddModule
            {
                SiteId = Guid.NewGuid(),
                PageId = Guid.NewGuid(),
                ModuleTypeId = Guid.NewGuid(),
                Zone = "Zone",
                SortOrder = 2,
                Title = "Title"
            };

            var page = PageFactory.Page(command.SiteId, command.PageId, "My Page");
            page.Events.Clear();

            var moduleRepositoryMock = new Mock<IModuleRepository>();

            var pageRepositoryMock = new Mock<IPageRepository>();
            pageRepositoryMock.Setup(x => x.GetById(command.SiteId, command.PageId)).Returns(page);
            pageRepositoryMock.Setup(x => x.Update(It.IsAny<Page>()));

            var createModuleValidatorMock = new Mock<IValidator<CreateModule>>();
            createModuleValidatorMock.Setup(x => x.Validate(It.IsAny<CreateModule>())).Returns(new ValidationResult());

            var addPageModuleValidatorMock = new Mock<IValidator<AddPageModule>>();
            addPageModuleValidatorMock.Setup(x => x.Validate(It.IsAny<AddPageModule>())).Returns(new ValidationResult());

            var addModuleHandler = new AddModuleHandler(moduleRepositoryMock.Object,
                pageRepositoryMock.Object,
                createModuleValidatorMock.Object,
                addPageModuleValidatorMock.Object);

            addModuleHandler.Handle(command);

            var @event = page.Events.OfType<PageModuleAdded>().SingleOrDefault();

            Assert.IsNotNull(@event);
        }

        [Test]
        public void Should_update_page()
        {
            var command = new AddModule
            {
                SiteId = Guid.NewGuid(),
                PageId = Guid.NewGuid(),
                ModuleTypeId = Guid.NewGuid(),
                Zone = "Zone",
                SortOrder = 1,
                Title = "Title"
            };

            var moduleRepositoryMock = new Mock<IModuleRepository>();
            var pageRepositoryMock = new Mock<IPageRepository>();
            pageRepositoryMock.Setup(x => x.GetById(command.SiteId, command.PageId)).Returns(new Page());
            pageRepositoryMock.Setup(x => x.Update(It.IsAny<Page>()));            
            var createModuleValidatorMock = new Mock<IValidator<CreateModule>>();
            createModuleValidatorMock.Setup(x => x.Validate(It.IsAny<CreateModule>())).Returns(new ValidationResult());
            var addPageModuleValidatorMock = new Mock<IValidator<AddPageModule>>();
            addPageModuleValidatorMock.Setup(x => x.Validate(It.IsAny<AddPageModule>())).Returns(new ValidationResult());

            var addModuleHandler = new AddModuleHandler(moduleRepositoryMock.Object,
                pageRepositoryMock.Object,
                createModuleValidatorMock.Object,
                addPageModuleValidatorMock.Object);

            addModuleHandler.Handle(command);

            pageRepositoryMock.Verify(x => x.Update(It.IsAny<Page>()));
        }

        [Test]
        public void Should_return_events()
        {
            var command = new AddModule
            {
                SiteId = Guid.NewGuid(),
                PageId = Guid.NewGuid(),
                ModuleTypeId = Guid.NewGuid(),
                Zone = "Zone",
                SortOrder = 1,
                Title = "Title"
            };

            var moduleRepositoryMock = new Mock<IModuleRepository>();
            var pageRepositoryMock = new Mock<IPageRepository>();
            pageRepositoryMock.Setup(x => x.GetById(command.SiteId, command.PageId)).Returns(new Page());
            var createModuleValidatorMock = new Mock<IValidator<CreateModule>>();
            createModuleValidatorMock.Setup(x => x.Validate(It.IsAny<CreateModule>())).Returns(new ValidationResult());
            var addPageModuleValidatorMock = new Mock<IValidator<AddPageModule>>();
            addPageModuleValidatorMock.Setup(x => x.Validate(It.IsAny<AddPageModule>())).Returns(new ValidationResult());

            var addModuleHandler = new AddModuleHandler(moduleRepositoryMock.Object,
                pageRepositoryMock.Object,
                createModuleValidatorMock.Object,
                addPageModuleValidatorMock.Object);

            var events = addModuleHandler.Handle(command);

            var enumerable = events as IList<IEvent> ?? events.ToList();
            Assert.AreEqual(typeof(ModuleCreated), enumerable.FirstOrDefault().GetType());
            Assert.AreEqual(typeof(PageModuleAdded), enumerable.Skip(1).FirstOrDefault().GetType());
        }
    }
}
