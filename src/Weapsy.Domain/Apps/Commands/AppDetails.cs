using System;
using Weapsy.Cqrs.Domain;

namespace Weapsy.Domain.Apps.Commands
{
    public class AppDetails : DomainCommand
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Folder { get; set; }
    }
}
