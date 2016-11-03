using System;
using Weapsy.Infrastructure.Domain;

namespace Weapsy.Domain.Pages.Events
{
    public class PageModuleRemoved : Event
    {
        public Guid SiteId { get; set; }
        public Guid ModuleId { get; set; }
        public Guid PageModuleId { get; set; }
    }
}
