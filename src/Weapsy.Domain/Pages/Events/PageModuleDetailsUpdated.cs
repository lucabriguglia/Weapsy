using System;
using Weapsy.Core.Domain;

namespace Weapsy.Domain.Model.Pages.Events
{
    public class PageModuleDetailsUpdated : Event
    {
        public Guid SiteId { get; set; }
        public PageModule PageModule { get; set; }
    }
}
