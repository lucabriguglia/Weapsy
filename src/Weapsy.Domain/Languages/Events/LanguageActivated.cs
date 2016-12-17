using System;
using Weapsy.Infrastructure.Domain;

namespace Weapsy.Domain.Languages.Events
{
    public class LanguageActivated : DomainEvent
    {
        public Guid SiteId { get; set; }
    }
}