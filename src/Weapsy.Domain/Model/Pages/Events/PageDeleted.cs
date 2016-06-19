using System;
using Weapsy.Core.Domain;

namespace Weapsy.Domain.Model.Pages.Events
{
    public class PageDeleted : Event
    {
        public Guid SiteId { get; set; }
    }
}