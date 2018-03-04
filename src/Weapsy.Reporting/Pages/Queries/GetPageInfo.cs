using System;
using Weapsy.Cqrs.Queries;

namespace Weapsy.Reporting.Pages.Queries
{
    public class GetPageInfo : IQuery
    {
        public Guid SiteId { get; set; }
        public Guid PageId { get; set; }
        public Guid LanguageId { get; set; } = Guid.NewGuid();
    }
}
