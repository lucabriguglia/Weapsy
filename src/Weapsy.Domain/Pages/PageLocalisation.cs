using System;
using Weapsy.Framework.Domain;

namespace Weapsy.Domain.Pages
{
    public class PageLocalisation : ValueObject
    {
        public Guid PageId { get; set; }
        public Guid LanguageId { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeywords { get; set; }
    }
}
