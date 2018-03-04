using System;
using Weapsy.Cqrs.Domain;

namespace Weapsy.Domain.Templates.Commands
{
    public class TemplateDetails : DomainCommand
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ViewName { get; set; }
        public TemplateType Type { get; set; }
        public Guid ThemeId { get; set; }
    }
}
