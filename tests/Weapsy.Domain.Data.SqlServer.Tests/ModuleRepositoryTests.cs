using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using Weapsy.Domain.Data.SqlServer.Repositories;
using Weapsy.Domain.Model.Modules;
using Weapsy.Tests.Factories;
using ModuleDbEntity = Weapsy.Domain.Data.SqlServer.Entities.Module;

namespace Weapsy.Domain.Data.SqlServer.Tests
{
    [TestFixture]
    public class ModuleRepositoryTests
    {
        private IModuleRepository _sut;
        private Guid _siteId;
        private Guid _moduleId1;
        private Guid _moduleId2;
        private Guid _moduleTypeId1;
        private Guid _moduleTypeId2;

        [SetUp]
        public void SetUp()
        {
            var optionsBuilder = new DbContextOptionsBuilder<WeapsyDbContext>();
            optionsBuilder.UseInMemoryDatabase();
            var dbContext = new WeapsyDbContext(optionsBuilder.Options);

            _siteId = Guid.NewGuid();
            _moduleId1 = Guid.NewGuid();
            _moduleId2 = Guid.NewGuid();
            _moduleTypeId1 = Guid.NewGuid();
            _moduleTypeId2 = Guid.NewGuid();

            dbContext.Set<ModuleDbEntity>().AddRange(
                new ModuleDbEntity
                {
                    SiteId = _siteId,
                    Id = _moduleId1,
                    ModuleTypeId = _moduleTypeId1,
                    Title = "Title 1",
                    Status = ModuleStatus.Active
                },
                new ModuleDbEntity
                {
                    SiteId = _siteId,
                    Id = _moduleId2,
                    ModuleTypeId = _moduleTypeId2,
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

            _sut = new ModuleRepository(dbContext, mapperMock.Object);
        }

        [Test]
        public void Should_return_module_by_id()
        {
            var actual = _sut.GetById(_moduleId1);
            Assert.NotNull(actual);
        }

        [Test]
        public void Should_return_all_modules()
        {
            var actual = _sut.GetAll();
            Assert.AreEqual(2, actual.Count);
        }

        [Test]
        public void Should_return_count_by_module_type_id()
        {
            var actual = _sut.GetCountByModuleTypeId(_moduleTypeId1);
            Assert.AreEqual(1, actual);
        }

        [Test]
        public void Should_return_count_by_module_id()
        {
            var actual = _sut.GetCountByModuleId(_moduleId1);
            Assert.AreEqual(1, actual);
        }

        [Test]
        public void Should_save_new_module()
        {
            var newModule = ModuleFactory.Module(_siteId, Guid.NewGuid(), Guid.NewGuid(), "Title 3");

            _sut.Create(newModule);

            var actual = _sut.GetById(newModule.Id);

            Assert.NotNull(actual);
        }

        [Test]
        public void Should_update_module()
        {
            var newModuleTitle = "New Title 1";

            var moduleToUpdate = ModuleFactory.Module(_siteId, Guid.NewGuid(), Guid.NewGuid(), newModuleTitle);

            _sut.Update(moduleToUpdate);

            var updatedModule = _sut.GetById(_moduleId1);

            Assert.AreEqual(newModuleTitle, updatedModule.Title);
        }
    }
}
