using System;
using Weapsy.Cqrs.Domain;

namespace Weapsy.Domain.Themes.Commands
{
    public class ThemeDetails : DomainCommand
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Folder { get; set; }
    }
}
