using System;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Weapsy.Data.Domain;
using Weapsy.Domain.Modules;
using Weapsy.Tests.Factories;
using ModuleDbEntity = Weapsy.Data.Entities.Module;

namespace Weapsy.Data.Tests.Repositories
{
    [TestFixture]
    public class ModuleRepositoryTests
    {
        private DbContextOptions<WeapsyDbContext> _contextOptions;
        private Guid _siteId;
        private Guid _moduleId1;
        private Guid _moduleId2;
        private Guid _moduleTypeId1;
        private Guid _moduleTypeId2;
        private Guid _deletedModuleId;

        [SetUp]
        public void SetUp()
        {
            _contextOptions = DbContextShared.CreateContextOptions();

            using (var context = new WeapsyDbContext(_contextOptions))
            {
                _siteId = Guid.NewGuid();
                _moduleId1 = Guid.NewGuid();
                _moduleId2 = Guid.NewGuid();
                _moduleTypeId1 = Guid.NewGuid();
                _moduleTypeId2 = Guid.NewGuid();
                _deletedModuleId = Guid.NewGuid();

                context.Modules.AddRange(
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
                        Id = _deletedModuleId,
                        Status = ModuleStatus.Deleted
                    }
                );

                context.SaveChanges();
            }
        }

        [Test]
        public void Should_return_null_if_module_is_deleted()
        {
            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new ModuleRepository(DbContextShared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                var module = repository.GetById(_deletedModuleId);

                Assert.Null(module);
            }
        }

        [Test]
        public void Should_return_module_by_id()
        {
            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new ModuleRepository(DbContextShared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                var module = repository.GetById(_moduleId1);

                Assert.NotNull(module);
            }
        }

        [Test]
        public void Should_return_count_by_module_type_id()
        {
            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new ModuleRepository(DbContextShared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                var count = repository.GetCountByModuleTypeId(_moduleTypeId1);

                Assert.AreEqual(1, count);
            }
        }

        [Test]
        public void Should_return_count_by_module_id()
        {
            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new ModuleRepository(DbContextShared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                var count = repository.GetCountByModuleId(_moduleId1);

                Assert.AreEqual(1, count);
            }
        }

        [Test]
        public void Should_save_new_module()
        {
            var newModule = ModuleFactory.Module(_siteId, Guid.NewGuid(), Guid.NewGuid(), "Title 3");

            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new ModuleRepository(DbContextShared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                repository.Create(newModule);
            }

            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new ModuleRepository(DbContextShared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                var module = repository.GetById(newModule.Id);

                Assert.NotNull(module);
            }
        }

        [Test]
        public void Should_update_module()
        {
            const string newModuleTitle = "New Title 1";

            var moduleToUpdate = ModuleFactory.Module(_siteId, _moduleTypeId1, _moduleId1, newModuleTitle);

            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new ModuleRepository(DbContextShared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                repository.Update(moduleToUpdate);
            }

            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new ModuleRepository(DbContextShared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                var updatedModule = repository.GetById(_moduleId1);

                Assert.AreEqual(newModuleTitle, updatedModule.Title);
            }
        }
    }
}
