using Weapsy.Infrastructure.Configuration;

namespace Weapsy.Data
{
    public interface IDataProvider
    {
        DataProvider Provider { get; }
        WeapsyDbContext DbContext();
    }
}
