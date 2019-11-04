using System;
using Weapsy.Cqrs.Domain;

namespace Weapsy.Domain.Pages.Events
{
    public class PageActivated : DomainEvent
    {
        public Guid SiteId { get; set; }
    }
}