using System;
using Weapsy.Infrastructure.Domain;

namespace Weapsy.Domain.Menus.Commands
{
    public class RemoveMenuItem : ICommand
    {
        public Guid SiteId { get; set; }
        public Guid MenuId { get; set; }
        public Guid MenuItemId { get; set; }
    }
}
