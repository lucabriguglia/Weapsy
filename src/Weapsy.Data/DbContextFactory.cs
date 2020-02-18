using Kledex.Dependencies;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using Weapsy.Data.Configuration;

namespace Weapsy.Data
{
    public class DbContextFactory : IDbContextFactory
    {
        private readonly IResolver _resolver;
        private readonly DataOptions _dataOptions;

        public DbContextFactory(IResolver resolver, IOptions<DataOptions> dataOptions)
        {
            _resolver = resolver;
            _dataOptions = dataOptions.Value;
        }

        public WeapsyDbContext Create()
        {
            var dataProvider = _resolver.ResolveAll<IDatabaseProvider>().SingleOrDefault(x => x.ProviderType.ToString() == _dataOptions.Provider);

            if (dataProvider == null)
                throw new Exception("The Database Provider entry in appsettings.json is empty or the one specified has not been found!");

            return dataProvider.CreateDbContext(_dataOptions.ConnectionString);
        }
    }
}
