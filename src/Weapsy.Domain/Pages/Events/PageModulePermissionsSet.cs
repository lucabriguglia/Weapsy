using System;
using System.Collections.Generic;
using Weapsy.Core.Domain;

namespace Weapsy.Domain.Pages.Events
{
    public class PageModulePermissionsSet : Event
    {
        public Guid SiteId { get; set; }
        public Guid PageModuleId { get; set; }
        public IList<PageModulePermission> PageModulePermissions { get; set; }
    }
}
