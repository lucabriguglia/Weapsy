using System;

namespace Weapsy.Domain.Modules.Commands
{
    public class DeleteModule : BaseSiteCommand
    {
        public Guid Id { get; set; }
    }
}
