using System;
using Weapsy.Infrastructure.Domain;

namespace Weapsy.Domain.Modules.Commands
{
    public class DeleteModule : ICommand
    {
        public Guid SiteId { get; set; }
        public Guid Id { get; set; }
    }
}
