using System;
using Weapsy.Framework.Domain;

namespace Weapsy.Domain.Pages
{
    public class PageModuleLocalisation : ValueObject
    {
        public Guid PageModuleId { get; set; }
        public Guid LanguageId { get; set; }
        public string Title { get; set; }
    }
}
