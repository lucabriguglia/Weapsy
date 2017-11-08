using System;
using Weapsy.Framework.Domain;

namespace Weapsy.Domain.Menus.Events
{
    public class MenuDeletedEvent : DomainEvent
    {
        public Guid SiteId { get; set; }
        public string Name { get; set; }
    }
}
