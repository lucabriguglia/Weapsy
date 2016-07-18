using System;
using System.Collections.Generic;
using Weapsy.Core.Domain;

namespace Weapsy.Domain.Services.Modules.Commands
{
    public class AddModule : ICommand
    {
        public Guid SiteId { get; set; }
        public Guid PageId { get; set; }
        public Guid ModuleTypeId { get; set; }
        public string Zone { get; set; }
        public int SortOrder { get; set; }
        public string Title { get; set; }
        public List<string> ViewPermissionRoleIds { get; set; }
    }
}
