using System;
using Weapsy.Cqrs.Domain;

namespace Weapsy.Apps.Text.Domain.Events
{
    public class TextModuleCreated : DomainEvent
    {
        public Guid SiteId { get; set; }
        public Guid ModuleId { get; set; }
        public Guid VersionId { get; set; }
        public string Content { get; set; }
        public TextModuleStatus Status { get; set; }
        public TextVersionStatus VersionStatus { get; set; }
    }
}
