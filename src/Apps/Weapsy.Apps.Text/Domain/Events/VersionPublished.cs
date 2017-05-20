using System;
using Weapsy.Framework.Domain;

namespace Weapsy.Apps.Text.Domain.Events
{
    public class VersionPublished : DomainEvent
    {
        public Guid SiteId { get; set; }
        public Guid ModuleId { get; set; }
        public Guid VersionId { get; set; }
    }
}
