using System;
using Weapsy.Cqrs.Queries;

namespace Weapsy.Reporting.Pages.Queries
{
    public class GetPageIdBySlug : IQuery
    {
        public Guid SiteId { get; set; }
        public string Slug { get; set; }
        public Guid LanguageId { get; set; } = Guid.Empty;
    }
}
