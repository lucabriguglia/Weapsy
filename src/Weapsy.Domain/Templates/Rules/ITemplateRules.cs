using System;
using Weapsy.Framework.Domain;

namespace Weapsy.Domain.Templates.Rules
{
    public interface ITemplateRules : IRules<Template>
    {
        bool DoesTemplateExist(Guid id);
        bool IsTemplateIdUnique(Guid id);
        bool IsTemplateNameUnique(string name, Guid templateId = new Guid());
        bool IsTemplateViewNameValid(string viewName);
        bool IsTemplateViewNameUnique(string viewName, Guid templateId = new Guid());
    }
}
