using System;
using Weapsy.Cqrs.Domain;

namespace Weapsy.Domain.Menus.Events
{
    public class MenuItemUpdated : DomainEvent
    {
        public Guid SiteId { get; set; }
        public string MenuName { get; set; }
        public MenuItem MenuItem { get; set; }
    }
}
