using System;

namespace Weapsy.Domain.Menus.Commands
{
    public class CreateMenu : BaseSiteCommand
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
