using System;
using Weapsy.Infrastructure.Domain;

namespace Weapsy.Domain.Menus.Events
{
    public class MenuCreated : DomainEvent
    {
        public Guid SiteId { get; set; }
        public string Name { get; set; }
        public MenuStatus Status { get; set; }
    }
}
