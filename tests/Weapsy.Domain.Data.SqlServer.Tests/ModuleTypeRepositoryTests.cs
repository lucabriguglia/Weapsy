using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using Moq;
using Weapsy.Domain.Data.SqlServer.Repositories;
using Weapsy.Domain.ModuleTypes;
using Weapsy.Tests.Factories;
using ModuleTypeDbEntity = Weapsy.Domain.Data.SqlServer.Entities.ModuleType;

namespace Weapsy.Domain.Data.SqlServer.Tests
{
    [TestFixture]
    public class ModuleTypeRepositoryTests
    {
        private IModuleTypeRepository _sut;
        private Guid _moduleTypeId1;
        private Guid _moduleTypeId2;

        [SetUp]
        public void SetUp()
        {
            var optionsBuilder = new DbContextOptionsBuilder<WeapsyDbContext>();
            optionsBuilder.UseInMemoryDatabase();
            var dbContext = new WeapsyDbContext(optionsBuilder.Options);

            _moduleTypeId1 = Guid.NewGuid();
            _moduleTypeId2 = Guid.NewGuid();

            dbContext.Set<ModuleTypeDbEntity>().AddRange(
                new ModuleTypeDbEntity
                {
                    Id = _moduleTypeId1,
                    Name = "Name 1",
                    Title = "Title 1",
                    Description = "Description 1",
                    Status = ModuleTypeStatus.Active
                },
                new ModuleTypeDbEntity
                {
                    Id = _moduleTypeId2,
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

            var mapperMock = new Mock<AutoMapper.IMapper>();
            mapperMock.Setup(x => x.Map<ModuleTypeDbEntity>(It.IsAny<ModuleType>())).Returns(new ModuleTypeDbEntity());
            mapperMock.Setup(x => x.Map<ModuleType>(It.IsAny<ModuleTypeDbEntity>())).Returns(new ModuleType());
            mapperMock.Setup(x => x.Map<ICollection<ModuleType>>(It.IsAny<ICollection<ModuleTypeDbEntity>>())).Returns(new List<ModuleType>
            {
                ModuleTypeFactory.ModuleType(_moduleTypeId1, "Name", "Title", "Description"),
                ModuleTypeFactory.ModuleType(_moduleTypeId2, "Name", "Title", "Description")
            });

            _sut = new ModuleTypeRepository(dbContext, mapperMock.Object);
        }

        [Test]
        public void Should_return_module_type_by_id()
        {
            var actual = _sut.GetById(_moduleTypeId1);
            Assert.NotNull(actual);
        }

        [Test]
        public void Should_return_module_type_by_name()
        {
            var actual = _sut.GetByName("Name 1");
            Assert.NotNull(actual);
        }

        [Test]
        public void Should_return_all_moduleTypes()
        {
            var actual = _sut.GetAll();
            Assert.AreEqual(2, actual.Count);
        }

        [Test]
        public void Should_save_new_moduleType()
        {
            var newModuleType = ModuleTypeFactory.ModuleType(Guid.NewGuid(), "Name 3", "Title 3", "Description 3");

            _sut.Create(newModuleType);

            var actual = _sut.GetById(newModuleType.Id);

            Assert.NotNull(actual);
        }

        [Test]
        public void Should_update_moduleType()
        {
            var newModuleTypeTitle = "New Title 1";

            var moduleTypeToUpdate = ModuleTypeFactory.ModuleType(Guid.NewGuid(), "Name 1", newModuleTypeTitle, "Description 1");

            _sut.Update(moduleTypeToUpdate);

            var updatedModuleType = _sut.GetById(_moduleTypeId1);

            Assert.AreEqual(newModuleTypeTitle, updatedModuleType.Title);
        }
    }
}
