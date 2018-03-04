using System;
using Weapsy.Cqrs.Domain;

namespace Weapsy.Domain.Templates.Commands
{
    public class DeleteTemplate : DomainCommand
    {
        public Guid Id { get; set; }
    }
}
