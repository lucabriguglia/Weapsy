using System;

namespace Weapsy.Domain.Menus.Commands
{
    public class CreateMenuCommand : BaseSiteCommand
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
