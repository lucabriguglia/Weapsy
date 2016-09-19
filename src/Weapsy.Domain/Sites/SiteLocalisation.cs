using System;
using Weapsy.Core.Domain;

namespace Weapsy.Domain.Model.Sites
{
    public class SiteLocalisation : ValueObject
    {
        public Guid SiteId { get; set; }
        public Guid LanguageId { get; set; }
        public string Title { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeywords { get; set; }
    }
}
