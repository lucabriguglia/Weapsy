using System;

namespace Weapsy.Domain.Templates
{
    public interface ITemplateRepository
    {
        Template GetById(Guid id);
        Template GetByName(string name);
        Template GetByViewName(string viewName);
        void Create(Template template);
        void Update(Template template);
    }
}
