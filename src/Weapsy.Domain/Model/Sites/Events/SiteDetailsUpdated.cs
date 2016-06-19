using System.Collections.Generic;
using Weapsy.Core.Domain;

namespace Weapsy.Domain.Model.Sites.Events
{
    public class SiteDetailsUpdated : Event
    {
        public string Url { get; set; }
        public string Title { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeywords { get; set; }
        public IList<SiteLocalisation> SiteLocalisations { get; set; } = new List<SiteLocalisation>();
    }
}
