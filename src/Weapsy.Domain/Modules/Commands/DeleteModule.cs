using System;
using Weapsy.Core.Domain;

namespace Weapsy.Domain.Model.Modules.Commands
{
    public class DeleteModule : ICommand
    {
        public Guid SiteId { get; set; }
        public Guid Id { get; set; }
    }
}
