using System;
using System.Collections.Generic;
using Weapsy.Core.Domain;

namespace Weapsy.Domain.Model.Pages.Events
{
    public class PagePermissionsSet : Event
    {
        public Guid SiteId { get; set; }
        public IList<PagePermission> PagePermissions { get; set; }
    }
}
