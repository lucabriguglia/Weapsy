using System;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Model.Pages;
using Weapsy.Domain.Services.Modules.Commands;
using Weapsy.Domain.Services.Modules.Handlers;
using Weapsy.Domain.Model.Modules;
using FluentValidation;
using Weapsy.Domain.Model.Modules.Commands;
using Weapsy.Domain.Model.Pages.Commands;
using FluentValidation.Results;
using Weapsy.Domain.Model.Modules.Events;
using System.Linq;
using Weapsy.Domain.Model.Pages.Events;

namespace Weapsy.Domain.Tests.Services.Handlers
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
        [Ignore("Non-virtual Member")]
        public void Should_add_module_to_page()
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

            var createModuleValidatorMock = new Mock<IValidator<CreateModule>>();
            createModuleValidatorMock.Setup(x => x.Validate(It.IsAny<CreateModule>())).Returns(new ValidationResult());
            var addPageModuleValidatorMock = new Mock<IValidator<AddPageModule>>();
            addPageModuleValidatorMock.Setup(x => x.Validate(It.IsAny<AddPageModule>())).Returns(new ValidationResult());

            var pageMock = new Mock<Page>();
            pageMock.Setup(x => x.AddModule(It.IsAny<AddPageModule>(), addPageModuleValidatorMock.Object));

            var moduleRepositoryMock = new Mock<IModuleRepository>();
            var pageRepositoryMock = new Mock<IPageRepository>();
            pageRepositoryMock.Setup(x => x.GetById(command.SiteId, command.PageId)).Returns(pageMock.Object);
            pageRepositoryMock.Setup(x => x.Update(It.IsAny<Page>()));

            var addModuleHandler = new AddModuleHandler(moduleRepositoryMock.Object,
                pageRepositoryMock.Object,
                createModuleValidatorMock.Object,
                addPageModuleValidatorMock.Object);

            addModuleHandler.Handle(command);

            pageMock.Verify(x => x.AddModule(It.IsAny<AddPageModule>(), addPageModuleValidatorMock.Object));
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

            Assert.AreEqual(2, events.Count);
            Assert.AreEqual(typeof(ModuleCreated), events.FirstOrDefault().GetType());
            Assert.AreEqual(typeof(PageModuleAdded), events.Skip(1).FirstOrDefault().GetType());
        }
    }
}
