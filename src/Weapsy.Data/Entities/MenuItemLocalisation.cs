using System;

namespace Weapsy.Data.Entities
{
    public class MenuItemLocalisation
    {
        public Guid MenuItemId { get; set; }
        public Guid LanguageId { get; set; }
        public string Text { get; set; }
        public string Title { get; set; }

        public virtual MenuItem MenuItem { get; set; }
        public virtual Language Language { get; set; }
    }
}
