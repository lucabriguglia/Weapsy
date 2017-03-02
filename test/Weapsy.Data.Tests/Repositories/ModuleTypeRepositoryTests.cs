using System;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Weapsy.Data.Domain;
using Weapsy.Domain.ModuleTypes;
using Weapsy.Tests.Factories;
using ModuleTypeDbEntity = Weapsy.Data.Entities.ModuleType;

namespace Weapsy.Data.Tests.Repositories
{
    [TestFixture]
    public class ModuleTypeRepositoryTests
    {
        private DbContextOptions<WeapsyDbContext> _contextOptions;
        private Guid _moduleTypeId1;
        private Guid _moduleTypeId2;
        private Guid _deletedModuleTypeId;

        [SetUp]
        public void SetUp()
        {
            _contextOptions = DbContextShared.CreateContextOptions();

            using (var context = new WeapsyDbContext(_contextOptions))
            {
                _moduleTypeId1 = Guid.NewGuid();
                _moduleTypeId2 = Guid.NewGuid();
                _deletedModuleTypeId = Guid.NewGuid();

                context.ModuleTypes.AddRange(
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
                        Id = _deletedModuleTypeId,
                        Status = ModuleTypeStatus.Deleted
                    }
                );

                context.SaveChanges();
            }
        }

        [Test]
        public void Should_return_null_if_module_type_is_deleted()
        {
            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new ModuleTypeRepository(DbContextShared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                var moduleType = repository.GetById(_deletedModuleTypeId);

                Assert.Null(moduleType);
            }
        }

        [Test]
        public void Should_return_module_type_by_id()
        {
            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new ModuleTypeRepository(DbContextShared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                var moduleType = repository.GetById(_moduleTypeId1);

                Assert.NotNull(moduleType);
            }
        }

        [Test]
        public void Should_return_module_type_by_name()
        {
            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new ModuleTypeRepository(DbContextShared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                var moduleType = repository.GetByName("Name 1");

                Assert.NotNull(moduleType);
            }
        }

        [Test]
        public void Should_save_new_moduleType()
        {
            var newModuleType = ModuleTypeFactory.ModuleType(Guid.NewGuid(), "Name 3", "Title 3", "Description 3");

            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new ModuleTypeRepository(DbContextShared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                repository.Create(newModuleType);
            }

            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new ModuleTypeRepository(DbContextShared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                var moduleType = repository.GetById(newModuleType.Id);

                Assert.NotNull(moduleType);
            }
        }

        [Test]
        public void Should_update_moduleType()
        {
            const string newModuleTypeTitle = "New Title 1";

            var moduleTypeToUpdate = ModuleTypeFactory.ModuleType(_moduleTypeId1, "Name 1", newModuleTypeTitle, "Description 1");

            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new ModuleTypeRepository(DbContextShared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                repository.Update(moduleTypeToUpdate);
            }

            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new ModuleTypeRepository(DbContextShared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                var updatedModuleType = repository.GetById(_moduleTypeId1);

                Assert.AreEqual(newModuleTypeTitle, updatedModuleType.Title);
            }
        }
    }
}
