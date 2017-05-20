using System;
using Weapsy.Framework.Commands;

namespace Weapsy.Domain.ModuleTypes.Commands
{
    public class ModuleTypeDetails : ICommand
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ViewType ViewType { get; set; }
        public string ViewName { get; set; }
        public EditType EditType { get; set; }
        public string EditUrl { get; set; }
    }
}
