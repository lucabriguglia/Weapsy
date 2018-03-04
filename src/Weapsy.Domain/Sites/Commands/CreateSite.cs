using System;
using Weapsy.Cqrs.Domain;

namespace Weapsy.Domain.Sites.Commands
{
    public class CreateSite : DomainCommand
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
