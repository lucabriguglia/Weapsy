using System;
using Weapsy.Framework.Domain;

namespace Weapsy.Domain.Pages.Events
{
    public class PageModuleDetailsUpdatedEvent : DomainEvent
    {
        public Guid SiteId { get; set; }
        public PageModule PageModule { get; set; }
    }
}
