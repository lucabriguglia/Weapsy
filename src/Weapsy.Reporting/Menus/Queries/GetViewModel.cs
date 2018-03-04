using System;
using Weapsy.Cqrs.Queries;

namespace Weapsy.Reporting.Menus.Queries
{
    public class GetViewModel: IQuery
    {
        public Guid SiteId { get; set; }
        public string Name { get; set; }
        public Guid LanguageId { get; set; } = Guid.NewGuid();
    }
}
