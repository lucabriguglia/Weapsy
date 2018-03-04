using System;
using Weapsy.Cqrs.Domain;
using Weapsy.Framework.Domain;

namespace Weapsy.Domain.Menus.Events
{
    public class MenuDeleted : DomainEvent
    {
        public Guid SiteId { get; set; }
        public string Name { get; set; }
    }
}
