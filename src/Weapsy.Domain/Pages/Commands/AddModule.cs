using System;
using System.Collections.Generic;
using Weapsy.Infrastructure.Domain;

namespace Weapsy.Domain.Pages.Commands
{
    public class AddModule : ICommand
    {
        public Guid SiteId { get; set; }
        public Guid PageId { get; set; }
        public Guid ModuleTypeId { get; set; }
        public string Zone { get; set; }
        public int SortOrder { get; set; }
        public string Title { get; set; }
        public IList<string> ViewPermissionRoleIds { get; set; }
    }
}
