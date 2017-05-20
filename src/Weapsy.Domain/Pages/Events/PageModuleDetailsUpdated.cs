using System;
using Weapsy.Framework.Domain;

namespace Weapsy.Domain.Pages.Events
{
    public class PageModuleDetailsUpdated : DomainEvent
    {
        public Guid SiteId { get; set; }
        public PageModule PageModule { get; set; }
    }
}
