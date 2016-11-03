using System;
using Weapsy.Infrastructure.Domain;

namespace Weapsy.Domain.Pages.Events
{
    public class PageHidden : Event
    {
        public Guid SiteId { get; set; }
    }
}