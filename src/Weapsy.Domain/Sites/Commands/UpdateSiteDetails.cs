using System;
using System.Collections.Generic;
using Weapsy.Core.Domain;

namespace Weapsy.Domain.Model.Sites.Commands
{
    public class UpdateSiteDetails : ICommand
    {
        public Guid SiteId { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeywords { get; set; }
        public List<SiteLocalisation> SiteLocalisations { get; set; } = new List<SiteLocalisation>();

        public class SiteLocalisation
        {
            public Guid LanguageId { get; set; }
            public string Title { get; set; }
            public string MetaDescription { get; set; }
            public string MetaKeywords { get; set; }
        }
    }
}
