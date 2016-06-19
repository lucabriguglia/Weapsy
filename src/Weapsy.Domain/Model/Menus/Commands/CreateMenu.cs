using System;
using Weapsy.Core.Domain;

namespace Weapsy.Domain.Model.Menus.Commands
{
    public class CreateMenu : ICommand
    {
        public Guid SiteId { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
