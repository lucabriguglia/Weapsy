using System;
using Weapsy.Domain.Model.Pages;

namespace Weapsy.Reporting.Pages
{
    public class PageAdminListModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public PageStatus Status { get; set; }
    }
}
