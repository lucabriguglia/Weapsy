using System;
using Weapsy.Framework.Domain;

namespace Weapsy.Domain.Languages.Events
{
    public class LanguageActivated : DomainEvent
    {
        public Guid SiteId { get; set; }
    }
}