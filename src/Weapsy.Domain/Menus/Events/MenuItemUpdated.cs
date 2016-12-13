using System;
using Weapsy.Infrastructure.Domain;

namespace Weapsy.Domain.Menus.Events
{
    public class MenuItemUpdated : DomainEvent
    {
        public Guid SiteId { get; set; }
        public string Name { get; set; }
        public MenuItem MenuItem { get; set; }
    }
}
