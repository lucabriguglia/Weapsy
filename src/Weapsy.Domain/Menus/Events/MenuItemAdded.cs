using System;
using Weapsy.Cqrs.Domain;

namespace Weapsy.Domain.Menus.Events
{
    public class MenuItemAdded : DomainEvent
    {
        public Guid SiteId { get; set; }
        public string Name { get; set; }
        public MenuItem MenuItem { get; set; }
    }
}
