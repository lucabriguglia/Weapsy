using System;

namespace Weapsy.Domain.Pages.Commands
{
    public class RemovePageModuleCommand : BaseSiteCommand
    {
        public Guid PageId { get; set; }
        public Guid ModuleId { get; set; }
    }
}
