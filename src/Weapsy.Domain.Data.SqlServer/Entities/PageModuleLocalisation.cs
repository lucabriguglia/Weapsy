using System;

namespace Weapsy.Domain.Data.SqlServer.Entities
{
    public class PageModuleLocalisation : IDbEntity
    {
        public Guid PageModuleId { get; set; }
        public Guid LanguageId { get; set; }
        public string Title { get; set; }

        public virtual PageModule PageModule { get; set; }
        public virtual Language Language { get; set; }
    }
}
