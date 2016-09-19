using System;
using System.Collections.Generic;
using Weapsy.Core.Domain;

namespace Weapsy.Domain.Model.Apps
{
    public interface IAppRepository : IRepository<App>
    {
        App GetById(Guid id);
        App GetByName(string name);
        App GetByFolder(string folder);
        ICollection<App> GetAll();
        void Create(App app);
        void Update(App app);
    }
}
