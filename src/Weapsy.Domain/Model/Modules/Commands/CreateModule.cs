using System;
using Weapsy.Core.Domain;

namespace Weapsy.Domain.Model.Modules.Commands
{
    public class CreateModule : ICommand
    {
        public Guid SiteId { get; set; }
        public Guid ModuleTypeId { get; set; }
        public Guid Id { get; set; }
        public string Title { get; set; }
    }
}
