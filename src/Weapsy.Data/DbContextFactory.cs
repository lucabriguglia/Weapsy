using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using Weapsy.Data.Configuration;

namespace Weapsy.Data
{
    public class DbContextFactory : IDbContextFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly DataOptions _dataOptions;

        public DbContextFactory(IServiceProvider serviceProvider, IOptions<DataOptions> dataOptions)
        {
            _serviceProvider = serviceProvider;
            _dataOptions = dataOptions.Value;
        }

        public WeapsyDbContext Create()
        {
            var dataProvider = _serviceProvider.GetServices<IDatabaseProvider>().SingleOrDefault(x => x.ProviderType.ToString() == _dataOptions.Provider);

            if (dataProvider == null)
                throw new Exception("The Database Provider entry in appsettings.json is empty or the one specified has not been found!");

            return dataProvider.CreateDbContext(_dataOptions.ConnectionString);
        }
    }
}
