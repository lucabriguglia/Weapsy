using System;
using Weapsy.Framework.Domain;

namespace Weapsy.Domain.Templates
{
    public interface ITemplateRepository : IRepository<Template>
    {
        Template GetById(Guid id);
        Template GetByName(string name);
        Template GetByViewName(string viewName);
        void Create(Template template);
        void Update(Template template);
    }
}
