using System;
using Weapsy.Core.Domain;

namespace Weapsy.Domain.Model.Menus.Events
{
    public class MenuDeleted : Event
    {
        public Guid SiteId { get; set; }
        public string Name { get; set; }
    }
}
