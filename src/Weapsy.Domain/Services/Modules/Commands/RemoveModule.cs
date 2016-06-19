using System;
using Weapsy.Core.Domain;

namespace Weapsy.Domain.Services.Modules.Commands
{
    public class RemoveModule : ICommand
    {
        public Guid SiteId { get; set; }
        public Guid PageId { get; set; }
        public Guid ModuleId { get; set; }
    }
}
