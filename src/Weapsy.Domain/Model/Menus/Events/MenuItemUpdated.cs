using System;
using Weapsy.Core.Domain;

namespace Weapsy.Domain.Model.Menus.Events
{
    public class MenuItemUpdated : Event
    {
        public Guid SiteId { get; set; }
        public MenuItem MenuItem { get; set; }
    }
}
