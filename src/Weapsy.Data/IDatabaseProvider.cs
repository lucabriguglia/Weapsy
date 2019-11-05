using Microsoft.Extensions.DependencyInjection;

namespace Weapsy.Data
{
    public interface IDatabaseProvider
    {
        Provider Provider { get; }
        IServiceCollection RegisterDbContext(IServiceCollection services, string connectionString);
        WeapsyDbContext CreateDbContext(string connectionString);
    }
}
