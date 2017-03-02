using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Weapsy.Data.Tests
{
    public static class DbContextShared
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

        public static IContextFactory CreateNewContextFactory(WeapsyDbContext context)
        {
            var dbContextFactoryMock = new Mock<IContextFactory>();
            dbContextFactoryMock.Setup(x => x.Create()).Returns(context);
            return dbContextFactoryMock.Object;
        }
    }
}
