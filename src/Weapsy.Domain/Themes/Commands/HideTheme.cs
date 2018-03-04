using System;
using Weapsy.Cqrs.Domain;

namespace Weapsy.Domain.Themes.Commands
{
    public class HideTheme : DomainCommand
    {
        public Guid Id { get; set; }
    }
}
