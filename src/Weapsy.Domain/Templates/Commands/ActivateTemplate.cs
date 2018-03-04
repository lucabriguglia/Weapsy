using System;
using Weapsy.Cqrs.Domain;

namespace Weapsy.Domain.Templates.Commands
{
    public class ActivateTemplate : DomainCommand
    {
        public Guid Id { get; set; }
    }
}
