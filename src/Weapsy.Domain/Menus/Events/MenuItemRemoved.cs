using System;
using Weapsy.Cqrs.Domain;
using Weapsy.Framework.Domain;

namespace Weapsy.Domain.Menus.Events
{
    public class MenuItemRemoved : DomainEvent
    {
        public Guid SiteId { get; set; }
        public string Name { get; set; }
        public Guid MenuItemId { get; set; }
    }
}
