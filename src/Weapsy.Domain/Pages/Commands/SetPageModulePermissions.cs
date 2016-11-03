using System;
using System.Collections.Generic;
using Weapsy.Infrastructure.Domain;

namespace Weapsy.Domain.Pages.Commands
{
    public class SetPageModulePermissions : ICommand
    {
        public Guid SiteId { get; set; }
        public Guid Id { get; set; }
        public Guid PageModuleId { get; set; }
        public IList<PageModulePermission> PageModulePermissions { get; set; } = new List<PageModulePermission>();
    }
}
