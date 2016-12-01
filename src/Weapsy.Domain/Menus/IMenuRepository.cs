using System;
using Weapsy.Infrastructure.Domain;

namespace Weapsy.Domain.Menus
{
    public interface IMenuRepository : IRepository<Menu>
    {
        Menu GetById(Guid id);
        Menu GetById(Guid siteId, Guid id);
        Menu GetByName(Guid siteId, string name);
        void Create(Menu menu);
        void Update(Menu menu);
    }
}
