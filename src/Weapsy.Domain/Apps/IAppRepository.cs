using System;
using System.Threading.Tasks;

namespace Weapsy.Domain.Apps
{
    public interface IAppRepository
    {
        App GetById(Guid id);
        App GetByName(string name);
        App GetByFolder(string folder);
        Task CreateAsync(App app);
        Task UpdateAsync(App app);
    }
}
