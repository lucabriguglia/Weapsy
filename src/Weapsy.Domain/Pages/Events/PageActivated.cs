using System;
using Weapsy.Framework.Domain;

namespace Weapsy.Domain.Pages.Events
{
    public class PageActivated : DomainEvent
    {
        public Guid SiteId { get; set; }
    }
}