using Weapsy.Core.Domain;

namespace Weapsy.Domain.Model.Templates.Events
{
    public class TemplateDetailsUpdated : Event
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ViewName { get; set; }
    }
}
