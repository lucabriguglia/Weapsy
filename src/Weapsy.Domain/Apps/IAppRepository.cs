using System;
using Weapsy.Framework.Domain;

namespace Weapsy.Domain.Apps
{
    public interface IAppRepository : IRepository<App>
    {
        App GetById(Guid id);
        App GetByName(string name);
        App GetByFolder(string folder);
        void Create(App app);
        void Update(App app);
    }
}
