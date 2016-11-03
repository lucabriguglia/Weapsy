using System;
using System.Collections.Generic;
using Weapsy.Infrastructure.Domain;

namespace Weapsy.Domain.Sites
{
    public interface ISiteRepository : IRepository<Site>
    {
        Site GetById(Guid id);
        Site GetByName(string name);
        Site GetByUrl(string url);
        ICollection<Site> GetAll();
        void Create(Site site);
        void Update(Site site);
    }
}
