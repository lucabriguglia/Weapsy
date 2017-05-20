using System;
using Weapsy.Framework.Queries;

namespace Weapsy.Reporting.Sites.Queries
{
    public class GetSiteInfo : IQuery
    {
        public string Name { get; set; }
        public Guid LanguageId { get; set; } = Guid.NewGuid();
    }
}
