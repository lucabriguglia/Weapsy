using System;
using Weapsy.Infrastructure.Domain;

namespace Weapsy.Domain.Pages.Events
{
    public class PageHidden : DomainEvent
    {
        public Guid SiteId { get; set; }
    }
}