using System;
using System.Collections.Generic;
using Weapsy.Core.Domain;

namespace Weapsy.Domain.Model.Menus.Commands
{
    public class ReorderMenuItems : ICommand
    {
        public Guid SiteId { get; set; }
        public Guid Id { get; set; }
        public IList<MenuItem> MenuItems { get; set; } = new List<MenuItem>();

        public class MenuItem
        {
            public Guid Id { get; set; }
            public Guid ParentId { get; set; }
        }
    }
}
