using System;
using System.Collections.Generic;
using Weapsy.Infrastructure.Domain;

namespace Weapsy.Domain.Pages
{
    public interface IPageRepository : IRepository<Page>
    {
        Page GetById(Guid id);
        Page GetById(Guid siteId, Guid id);
        Page GetByName(Guid siteId, string name);
        Page GetByUrl(Guid siteId, string url);
        Guid? GetIdBySlug(Guid siteId, string slug);
        Guid? GetIdBySlug(Guid siteId, string slug, Guid languageId);
        ICollection<Page> GetAll(Guid siteId);
        void Create(Page page);
        void Update(Page page);
    }
}
