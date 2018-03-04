using System;

namespace Weapsy.Domain.Apps
{
    public interface IAppRepository
    {
        App GetById(Guid id);
        App GetByName(string name);
        App GetByFolder(string folder);
        void Create(App app);
        void Update(App app);
    }
}
