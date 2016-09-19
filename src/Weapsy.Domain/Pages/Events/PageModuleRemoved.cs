using System;
using Weapsy.Core.Domain;

namespace Weapsy.Domain.Model.Pages.Events
{
    public class PageModuleRemoved : Event
    {
        public Guid SiteId { get; set; }
        public Guid ModuleId { get; set; }
        public Guid PageModuleId { get; set; }
    }
}
