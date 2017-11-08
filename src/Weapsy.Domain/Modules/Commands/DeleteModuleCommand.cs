using System;

namespace Weapsy.Domain.Modules.Commands
{
    public class DeleteModuleCommand : BaseSiteCommand
    {
        public Guid Id { get; set; }
    }
}
