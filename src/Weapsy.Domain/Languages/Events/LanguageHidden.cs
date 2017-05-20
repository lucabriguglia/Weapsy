using System;
using Weapsy.Framework.Domain;

namespace Weapsy.Domain.Languages.Events
{
    public class LanguageHidden : DomainEvent
    {
        public Guid SiteId { get; set; }
    }
}