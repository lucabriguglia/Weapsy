using System;
using Weapsy.Core.Domain;

namespace Weapsy.Domain.Model.Pages.Events
{
    public class PageActivated : Event
    {
        public Guid SiteId { get; set; }
    }
}