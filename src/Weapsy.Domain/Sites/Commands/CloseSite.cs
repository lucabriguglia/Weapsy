using System;
using Weapsy.Cqrs.Domain;

namespace Weapsy.Domain.Sites.Commands
{
    public class CloseSite : DomainCommand
    {
        public Guid Id { get; set; }
    }
}
