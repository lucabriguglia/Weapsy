using System;
using Weapsy.Core.Domain;

namespace Weapsy.Domain.Model.Templates.Commands
{
    public class TemplateDetails : ICommand
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ViewName { get; set; }
    }
}
