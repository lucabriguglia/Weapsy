using System;
using System.Collections.Generic;
using Weapsy.Core.Domain;

namespace Weapsy.Domain.Model.Templates
{
    public interface ITemplateRepository : IRepository<Template>
    {
        Template GetById(Guid id);
        Template GetByName(string name);
        Template GetByViewName(string viewName);
        IEnumerable<Template> GetAll();
        void Create(Template template);
        void Update(Template template);
    }
}
