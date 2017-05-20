using System;
using Weapsy.Framework.Domain;

namespace Weapsy.Domain.Pages.Events
{
    public class PageHidden : DomainEvent
    {
        public Guid SiteId { get; set; }
    }
}