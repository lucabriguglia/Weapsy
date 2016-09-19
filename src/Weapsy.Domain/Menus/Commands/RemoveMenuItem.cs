using System;
using Weapsy.Core.Domain;

namespace Weapsy.Domain.Model.Menus.Commands
{
    public class RemoveMenuItem : ICommand
    {
        public Guid SiteId { get; set; }
        public Guid MenuId { get; set; }
        public Guid MenuItemId { get; set; }
    }
}
