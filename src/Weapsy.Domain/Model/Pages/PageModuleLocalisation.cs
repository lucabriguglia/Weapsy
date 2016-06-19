using System;
using Weapsy.Core.Domain;

namespace Weapsy.Domain.Model.Pages
{
    public class PageModuleLocalisation : ValueObject
    {
        public Guid PageModuleId { get; set; }
        public Guid LanguageId { get; set; }
        public string Title { get; set; }
    }
}
