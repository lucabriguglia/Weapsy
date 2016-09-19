using System;
using System.Text.RegularExpressions;

namespace Weapsy.Domain.Templates.Rules
{
    public class TemplateRules : ITemplateRules
    {
        private readonly ITemplateRepository _templateRepository;

        public TemplateRules(ITemplateRepository templateRepository)
        {
            _templateRepository = templateRepository;
        }

        public bool DoesTemplateExist(Guid id)
        {
            var template = _templateRepository.GetById(id);
            return template != null && template.Status != TemplateStatus.Deleted;
        }

        public bool IsTemplateIdUnique(Guid id)
        {
            return _templateRepository.GetById(id) == null;
        }

        public bool IsTemplateNameUnique(string name, Guid templateId = new Guid())
        {
            var template = _templateRepository.GetByName(name);
            return IsTemplateUnique(template, templateId);
        }

        public bool IsTemplateViewNameValid(string viewName)
        {
            if (string.IsNullOrWhiteSpace(viewName)) return false;
            var regex = new Regex(@"^[A-Za-z\d_-]+$");
            var match = regex.Match(viewName);
            return match.Success;
        }

        public bool IsTemplateViewNameUnique(string viewName, Guid templateId = new Guid())
        {
            var template = _templateRepository.GetByViewName(viewName);
            return IsTemplateUnique(template, templateId);
        }

        private bool IsTemplateUnique(Template template, Guid templateId)
        {
            return template == null
                || template.Status == TemplateStatus.Deleted
                || (templateId != Guid.Empty && template.Id == templateId);
        }
    }
}
