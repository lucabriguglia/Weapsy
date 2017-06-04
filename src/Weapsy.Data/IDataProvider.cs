using Microsoft.Extensions.DependencyInjection;
using Weapsy.Data.Configuration;

namespace Weapsy.Data
{
    public interface IDataProvider
    {
        DataProvider Provider { get; }
        IServiceCollection RegisterDbContext(IServiceCollection services, string connectionString);
        WeapsyDbContext CreateDbContext(string connectionString);
    }
}
