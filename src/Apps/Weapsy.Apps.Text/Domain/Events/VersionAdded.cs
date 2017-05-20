using System;
using Weapsy.Framework.Domain;

namespace Weapsy.Apps.Text.Domain.Events
{
    public class VersionAdded : DomainEvent
    {
        public Guid SiteId { get; set; }
        public Guid ModuleId { get; set; }
        public Guid VersionId { get; set; }
        public string Content { get; set; }
        public string Description { get; set; }
        public TextVersionStatus Status { get; set; }
    }
}
