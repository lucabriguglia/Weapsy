using Kledex.Dependencies;
using System;
using System.Linq;

namespace Weapsy.Data
{
    public class DbContextFactory : IDbContextFactory
    {
        private readonly IResolver _resolver;

        public DbContextFactory(IResolver resolver)
        {
            _resolver = resolver;
        }

        public WeapsyDbContext Create()
        {
            var dataProvider = _resolver.ResolveAll<IDatabaseProvider>().SingleOrDefault(x => x.Provider == Provider.SqlServer);

            if (dataProvider == null)
                throw new Exception("The Database Provider entry in appsettings.json is empty or the one specified has not been found!");

            return dataProvider.CreateDbContext("Server=(localdb)\\mssqllocaldb;Database=WeapsyDb;Trusted_Connection=True;MultipleActiveResultSets=true");
        }
    }
}
