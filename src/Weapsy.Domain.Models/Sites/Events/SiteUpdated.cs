using System;
using Kledex.Events;

namespace Weapsy.Domain.Models.Sites.Events
{
    public class SiteUpdated :Event
    {
        public Guid Id { get; set; }
    }
}
