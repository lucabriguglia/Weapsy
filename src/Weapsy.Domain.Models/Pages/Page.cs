using System;

namespace Weapsy.Domain.Models.Pages
{
    public class Page
    {
        public Guid Id { get; set; }
        public Guid SiteId { get; set; }
        public string Name { get; set; }
        public PageStatus Status { get; set; }

        public Page()
        {
        }
    }
}
