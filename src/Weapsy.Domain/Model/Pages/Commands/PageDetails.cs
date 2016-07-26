using System;
using System.Collections.Generic;
using Weapsy.Core.Domain;

namespace Weapsy.Domain.Model.Pages.Commands
{
    public class PageDetails : ICommand
    {
        public Guid SiteId { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeywords { get; set; }
        public List<PageLocalisation> PageLocalisations { get; set; } = new List<PageLocalisation>();
        public List<PagePermission> PagePermissions { get; set; } = new List<PagePermission>();

        public class PageLocalisation
        {
            public Guid LanguageId { get; set; }
            public string Url { get; set; }
            public string Title { get; set; }
            public string MetaDescription { get; set; }
            public string MetaKeywords { get; set; }
        }
    }
}
