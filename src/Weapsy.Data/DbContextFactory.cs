using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Weapsy.Infrastructure.DependencyResolver;

namespace Weapsy.Data
{
    public class DbContextFactory : IDbContextFactory
    {
        private Infrastructure.Configuration.Data DataConfiguration { get; }
        private readonly IResolver _resolver;

        public DbContextFactory(IOptions<Infrastructure.Configuration.Data> dataConfiguration, 
            IResolver resolver)
        {
            DataConfiguration = dataConfiguration.Value;
            _resolver = resolver;            
        }

        public WeapsyDbContext Create()
        {
            var dataProvider = _resolver.ResolveAll<IDataProvider>().SingleOrDefault(x => x.Provider == DataConfiguration.Provider);

            if (dataProvider == null)
                throw new Exception("The DataProvider in appsettings.json is empty or the one specified has not been found!");

            return dataProvider.DbContext();
        }

        public WeapsyDbContext Create(DbContextOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
