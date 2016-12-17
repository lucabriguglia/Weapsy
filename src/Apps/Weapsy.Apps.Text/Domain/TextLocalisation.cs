using System;
using Weapsy.Infrastructure.Domain;

namespace Weapsy.Apps.Text.Domain
{
    public class TextLocalisation : ValueObject
    {
        public Guid TextVersionId { get; set; }
        public Guid LanguageId { get; set; }
        public string Content { get; set; }
    }
}
