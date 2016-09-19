using System;
using Weapsy.Core.Domain;

namespace Weapsy.Domain.Pages.Events
{
    public class PageHidden : Event
    {
        public Guid SiteId { get; set; }
    }
}