using System;
using System.Collections.Generic;
using Weapsy.Cqrs.Domain;

namespace Weapsy.Domain.Pages.Events
{
    public class PagePermissionsSet : DomainEvent
    {
        public Guid SiteId { get; set; }
        public IList<PagePermission> PagePermissions { get; set; }
    }
}
