using System;
using Weapsy.Cqrs.Domain;
using Weapsy.Framework.Domain;

namespace Weapsy.Domain.Menus.Events
{
    public class MenuCreated : DomainEvent
    {
        public Guid SiteId { get; set; }
        public string Name { get; set; }
        public MenuStatus Status { get; set; }
    }
}
