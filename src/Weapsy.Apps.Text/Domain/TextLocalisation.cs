using System;
using Weapsy.Framework.Domain;

namespace Weapsy.Apps.Text.Domain
{
    public class TextLocalisation : ValueObject
    {
        public Guid TextVersionId { get; set; }
        public Guid LanguageId { get; set; }
        public string Content { get; set; }
    }
}
