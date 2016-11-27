using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Weapsy.Data;

namespace Weapsy.Domain.Data.Tests
{
    public static class Shared
    {
        public static DbContextOptions<WeapsyDbContext> CreateContextOptions()
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            // Create a new options instance telling the context to use an
            // InMemory database and the new service provider.
            var builder = new DbContextOptionsBuilder<WeapsyDbContext>();
            builder.UseInMemoryDatabase()
                   .UseInternalServiceProvider(serviceProvider);

            return builder.Options;
        }

        public static IWeapsyDbContextFactory CreateNewContextFactory(WeapsyDbContext context)
        {
            var dbContextFactoryMock = new Mock<IWeapsyDbContextFactory>();
            dbContextFactoryMock.Setup(x => x.Create()).Returns(context);
            return dbContextFactoryMock.Object;
        }

        public static IMapper CreateNewMapper()
        {
            var autoMapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });

            return autoMapperConfig.CreateMapper();
        }
    }
}
