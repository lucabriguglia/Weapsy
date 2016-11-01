using System;
using Weapsy.Core.Domain;

namespace Weapsy.Domain.Pages.Commands
{
    public class AddPageModule : ICommand
    {
        public Guid SiteId { get; set; }
        public Guid PageId { get; set; }
        public Guid ModuleId { get; set; }
        public Guid PageModuleId { get; set; }
        public string Title { get; set; }
        public string Zone { get; set; }
        public int SortOrder { get; set; }
    }
}