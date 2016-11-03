using System;

namespace Weapsy.Domain.Modules.Commands
{
    public class CreateModule : BaseSiteCommand
    {
        public Guid ModuleTypeId { get; set; }
        public Guid Id { get; set; }
        public string Title { get; set; }
    }
}
