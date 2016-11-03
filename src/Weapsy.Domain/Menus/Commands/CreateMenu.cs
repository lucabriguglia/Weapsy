using System;
using Weapsy.Infrastructure.Domain;

namespace Weapsy.Domain.Menus.Commands
{
    public class CreateMenu : ICommand
    {
        public Guid SiteId { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
