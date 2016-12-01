using System;
using Weapsy.Infrastructure.Domain;

namespace Weapsy.Domain.Pages
{
    public interface IPageRepository : IRepository<Page>
    {
        Page GetById(Guid id);
        Page GetById(Guid siteId, Guid id);
        Page GetByName(Guid siteId, string name);
        Page GetByUrl(Guid siteId, string url);
        void Create(Page page);
        void Update(Page page);
    }
}
