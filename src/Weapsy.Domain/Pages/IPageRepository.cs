using System;
using Weapsy.Framework.Domain;

namespace Weapsy.Domain.Pages
{
    public interface IPageRepository : IRepository<Page>
    {
        Page GetById(Guid id);
        Page GetById(Guid siteId, Guid id);
        Guid GetPageIdByName(Guid siteId, string name);
        Guid GetPageIdBySlug(Guid siteId, string slug);
        Guid GetPageIdByLocalisedSlug(Guid siteId, string slug);
        void Create(Page page);
        void Update(Page page);
    }
}
