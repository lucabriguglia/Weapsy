using Weapsy.Framework.Domain;

namespace Weapsy.Domain.Templates.Events
{
    public class TemplateCreated : DomainEvent
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ViewName { get; set; }
        public TemplateStatus Status { get; set; }
    }
}
