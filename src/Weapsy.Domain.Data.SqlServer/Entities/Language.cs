using System;
using Weapsy.Domain.Languages;

namespace Weapsy.Domain.Data.SqlServer.Entities
{
    public class Language : IDbEntity
    {
        public Guid SiteId { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string CultureName { get; set; }
        public string Url { get; set; }
        public int SortOrder { get; set; }
        public LanguageStatus Status { get; set; }

        public virtual Site Site { get; set; }
    }
}