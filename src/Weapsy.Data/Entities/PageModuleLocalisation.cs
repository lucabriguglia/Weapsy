using System;

namespace Weapsy.Data.Entities
{
    public class PageModuleLocalisation
    {
        public Guid PageModuleId { get; set; }
        public Guid LanguageId { get; set; }
        public string Title { get; set; }

        public virtual PageModule PageModule { get; set; }
        public virtual Language Language { get; set; }
    }
}
