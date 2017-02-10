using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Options;
using Weapsy.Infrastructure.Configuration;
using Weapsy.Infrastructure.DependencyResolver;

namespace Weapsy.Data
{
    public class DbContextFactory : IDbContextFactory
    {
        private Weapsy.Infrastructure.Configuration.Data DataConfiguration { get; }
        private Weapsy.Infrastructure.Configuration.ConnectionStrings ConnectionStrings { get; }
        private readonly IResolver _resolver;
        private const string ErrorMessage = "The Data Provider entry in appsettings.json is empty or the one specified has not been found!";

        public DbContextFactory(IOptions<Weapsy.Infrastructure.Configuration.Data> dataOptions,
            IResolver resolver, IOptions<ConnectionStrings> connectionStringsOption)
        {
            DataConfiguration = dataOptions.Value;
            ConnectionStrings = connectionStringsOption.Value;
            _resolver = resolver;
        }

        public WeapsyDbContext Create()
        {
            var dataProvider = _resolver.ResolveAll<IDataProvider>().SingleOrDefault(x => x.Provider == DataConfiguration.Provider);

            if (dataProvider == null)
                throw new Exception(ErrorMessage);

            return dataProvider.CreateDbContext(ConnectionStrings.DefaultConnection);
        }
    }

    public class MigrationsDbContextFactory : IDbContextFactory<WeapsyDbContext>
    {
        public WeapsyDbContext Create(DbContextFactoryOptions options)
        {
            var builder = new DbContextOptionsBuilder<WeapsyDbContext>();
            builder.UseSqlServer("UsedForMigrationsOnlyUntilClassLibraryBugIsFixed");

            return new WeapsyDbContext(builder.Options);
        }
    }
}
