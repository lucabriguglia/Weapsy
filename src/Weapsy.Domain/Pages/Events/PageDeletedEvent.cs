using System;
using Weapsy.Framework.Domain;

namespace Weapsy.Domain.Pages.Events
{
    public class PageDeletedEvent : DomainEvent
    {
        public Guid SiteId { get; set; }
    }
}