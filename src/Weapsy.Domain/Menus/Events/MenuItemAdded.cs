using System;
using Weapsy.Core.Domain;

namespace Weapsy.Domain.Menus.Events
{
    public class MenuItemAdded : Event
    {
        public Guid SiteId { get; set; }
        public string Name { get; set; }
        public MenuItem MenuItem { get; set; }
    }
}
