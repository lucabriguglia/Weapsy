using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using Weapsy.Domain.Data.Repositories;
using Weapsy.Domain.Model.ModuleTypes;
using Weapsy.Tests.Factories;
using ModuleTypeDbEntity = Weapsy.Domain.Data.Entities.ModuleType;

namespace Weapsy.Domain.Data.SqlServer.Tests
{
    [TestFixture]
    public class ModuleTypeRepositoryTests
    {
        private IModuleTypeRepository sut;
        private Guid moduleTypeId1;
        private Guid moduleTypeId2;

        [SetUp]
        public void SetUp()
        {
            var optionsBuilder = new DbContextOptionsBuilder<WeapsyDbContext>();
            optionsBuilder.UseInMemoryDatabase();
            var dbContext = new WeapsyDbContext(optionsBuilder.Options);

            moduleTypeId1 = Guid.NewGuid();
            moduleTypeId2 = Guid.NewGuid();

            dbContext.Set<ModuleTypeDbEntity>().AddRange(
                new ModuleTypeDbEntity
                {
                    Id = moduleTypeId1,
                    Name = "Name 1",
                    Title = "Title 1",
                    Description = "Description 1",
                    Status = ModuleTypeStatus.Active
                },
                new ModuleTypeDbEntity
                {
                    Id = moduleTypeId2,
                    Name = "Name 2",
                    Title = "Title 2",
                    Description = "Description 2",
                    Status = ModuleTypeStatus.Active
                },
                new ModuleTypeDbEntity
                {
                    Status = ModuleTypeStatus.Deleted
                }
            );

            dbContext.SaveChanges();

            var mapperMock = new Moq.Mock<AutoMapper.IMapper>();
            mapperMock.Setup(x => x.Map<ModuleTypeDbEntity>(Moq.It.IsAny<ModuleType>())).Returns(new ModuleTypeDbEntity());
            mapperMock.Setup(x => x.Map<ModuleType>(Moq.It.IsAny<ModuleTypeDbEntity>())).Returns(new ModuleType());

            sut = new ModuleTypeRepository(dbContext, mapperMock.Object);
        }

        [Test]
        public void Should_return_module_type_by_id()
        {
            var actual = sut.GetById(moduleTypeId1);
            Assert.NotNull(actual);
        }

        [Test]
        public void Should_return_module_type_by_name()
        {
            var actual = sut.GetByName("Name 1");
            Assert.NotNull(actual);
        }

        [Test]
        public void Should_return_all_moduleTypes()
        {
            var actual = sut.GetAll();
            Assert.AreEqual(2, actual.Count);
        }

        [Test]
        public void Should_save_new_moduleType()
        {
            var newModuleType = ModuleTypeFactory.ModuleType(Guid.NewGuid(), "Name 3", "Title 3", "Description 3");

            sut.Create(newModuleType);

            var actual = sut.GetById(newModuleType.Id);

            Assert.NotNull(actual);
        }

        [Test]
        public void Should_update_moduleType()
        {
            var newModuleTypeTitle = "New Title 1";

            var moduleTypeToUpdate = ModuleTypeFactory.ModuleType(Guid.NewGuid(), "Name 1", newModuleTypeTitle, "Description 1");

            sut.Update(moduleTypeToUpdate);

            var updatedModuleType = sut.GetById(moduleTypeId1);

            Assert.AreEqual(newModuleTypeTitle, updatedModuleType.Title);
        }
    }
}
