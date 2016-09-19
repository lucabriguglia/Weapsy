using System;
using Weapsy.Core.Domain;

namespace Weapsy.Domain.Menus.Events
{
    public class MenuItemRemoved : Event
    {
        public Guid SiteId { get; set; }
        public string Name { get; set; }
        public Guid MenuItemId { get; set; }
    }
}
