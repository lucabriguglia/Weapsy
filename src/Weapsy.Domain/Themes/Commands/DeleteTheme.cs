using System;
using Weapsy.Cqrs.Domain;

namespace Weapsy.Domain.Themes.Commands
{
    public class DeleteTheme : DomainCommand
    {
        public Guid Id { get; set; }
    }
}
