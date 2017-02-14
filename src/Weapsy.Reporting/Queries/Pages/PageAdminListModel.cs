using System;
using System.Collections.Generic;
using Weapsy.Domain.Pages;

namespace Weapsy.Reporting.Pages
{
    public class PageAdminListModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public PageStatus Status { get; set; }
        public List<PageLocalisationAdminListModel> PageLocalisations { get; set; } = new List<PageLocalisationAdminListModel>();
    }

    public class PageLocalisationAdminListModel
    {
        public Guid LanguageId { get; set; }
        public string Url { get; set; }
    }
}
