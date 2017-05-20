using System;
using Weapsy.Framework.Commands;

namespace Weapsy.Domain.Templates.Commands
{
    public class TemplateDetails : ICommand
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ViewName { get; set; }
    }
}
