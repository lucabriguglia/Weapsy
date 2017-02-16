using System;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Weapsy.Domain.ModuleTypes;
using ModuleType = Weapsy.Data.Entities.ModuleType;

namespace Weapsy.Data.Tests.Reporting
{
    [TestFixture]
    public class ModuleTypeHandlersTests
    {
        private DbContextOptions<WeapsyDbContext> _contextOptions;
        private Guid _moduleTypeId;

        [SetUp]
        public void Setup()
        {
            _contextOptions = DbContextShared.CreateContextOptions();

            using (var context = new WeapsyDbContext(_contextOptions))
            {
                _moduleTypeId = Guid.NewGuid();

                context.ModuleTypes.AddRange(
                    new ModuleType
                    {
                        Id = _moduleTypeId,
                        Name = "ModuleType 1",
                        Status = ModuleTypeStatus.Active
                    }
                );

                context.SaveChanges();
            }
        }
    }
}
