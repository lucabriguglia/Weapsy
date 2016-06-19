using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using Weapsy.Domain.Data.Repositories;
using Weapsy.Domain.Model.Modules;
using Weapsy.Tests.Factories;
using ModuleDbEntity = Weapsy.Domain.Data.Entities.Module;

namespace Weapsy.Domain.Data.SqlServer.Tests
{
    [TestFixture]
    public class ModuleRepositoryTests
    {
        private IModuleRepository sut;
        private Guid siteId;
        private Guid moduleId1;
        private Guid moduleId2;
        private Guid moduleTypeId1;
        private Guid moduleTypeId2;

        [SetUp]
        public void SetUp()
        {
            var optionsBuilder = new DbContextOptionsBuilder<WeapsyDbContext>();
            optionsBuilder.UseInMemoryDatabase();
            var dbContext = new WeapsyDbContext(optionsBuilder.Options);

            siteId = Guid.NewGuid();
            moduleId1 = Guid.NewGuid();
            moduleId2 = Guid.NewGuid();
            moduleTypeId1 = Guid.NewGuid();
            moduleTypeId2 = Guid.NewGuid();

            dbContext.Set<ModuleDbEntity>().AddRange(
                new ModuleDbEntity
                {
                    SiteId = siteId,
                    Id = moduleId1,
                    ModuleTypeId = moduleTypeId1,
                    Title = "Title 1",
                    Status = ModuleStatus.Active
                },
                new ModuleDbEntity
                {
                    SiteId = siteId,
                    Id = moduleId2,
                    ModuleTypeId = moduleTypeId2,
                    Title = "Title 2",
                    Status = ModuleStatus.Active
                },
                new ModuleDbEntity
                {
                    Status = ModuleStatus.Deleted
                }
            );

            dbContext.SaveChanges();

            var mapperMock = new Moq.Mock<AutoMapper.IMapper>();
            mapperMock.Setup(x => x.Map<ModuleDbEntity>(Moq.It.IsAny<Module>())).Returns(new ModuleDbEntity());
            mapperMock.Setup(x => x.Map<Module>(Moq.It.IsAny<ModuleDbEntity>())).Returns(new Module());

            sut = new ModuleRepository(dbContext, mapperMock.Object);
        }

        [Test]
        public void Should_return_module_by_id()
        {
            var actual = sut.GetById(moduleId1);
            Assert.NotNull(actual);
        }

        [Test]
        public void Should_return_all_modules()
        {
            var actual = sut.GetAll();
            Assert.AreEqual(2, actual.Count);
        }

        [Test]
        public void Should_return_count_by_module_type_id()
        {
            var actual = sut.GetCountByModuleTypeId(moduleTypeId1);
            Assert.AreEqual(1, actual);
        }

        [Test]
        public void Should_return_count_by_module_id()
        {
            var actual = sut.GetCountByModuleId(moduleId1);
            Assert.AreEqual(1, actual);
        }

        [Test]
        public void Should_save_new_module()
        {
            var newModule = ModuleFactory.Module(siteId, Guid.NewGuid(), Guid.NewGuid(), "Title 3");

            sut.Create(newModule);

            var actual = sut.GetById(newModule.Id);

            Assert.NotNull(actual);
        }

        [Test]
        public void Should_update_module()
        {
            var newModuleTitle = "New Title 1";

            var moduleToUpdate = ModuleFactory.Module(siteId, Guid.NewGuid(), Guid.NewGuid(), newModuleTitle);

            sut.Update(moduleToUpdate);

            var updatedModule = sut.GetById(moduleId1);

            Assert.AreEqual(newModuleTitle, updatedModule.Title);
        }
    }
}
