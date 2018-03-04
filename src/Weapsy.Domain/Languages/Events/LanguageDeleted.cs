using System;
using Weapsy.Cqrs.Domain;

namespace Weapsy.Domain.Languages.Events
{
    public class LanguageDeleted : DomainEvent
    {
        public Guid SiteId { get; set; }
    }
}