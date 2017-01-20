using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Weapsy.Data;
using Weapsy.Data.Providers.MSSQL;

namespace Weapsy.Tests.Shared
{
    public static class DbContextShared
    {
        public static DbContextOptions<MSSQLDbContext> CreateContextOptions()
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            // Create a new options instance telling the context to use an
            // InMemory database and the new service provider.
            var builder = new DbContextOptionsBuilder<MSSQLDbContext>();
            builder.UseInMemoryDatabase()
                   .UseInternalServiceProvider(serviceProvider);

            return builder.Options;
        }

        public static IDbContextFactory CreateNewContextFactory(MSSQLDbContext context)
        {
            var dbContextFactoryMock = new Mock<IDbContextFactory>();
            dbContextFactoryMock.Setup(x => x.Create()).Returns(context);
            return dbContextFactoryMock.Object;
        }
    }
}
