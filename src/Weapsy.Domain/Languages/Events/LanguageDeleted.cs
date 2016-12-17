using System;
using Weapsy.Infrastructure.Domain;

namespace Weapsy.Domain.Languages.Events
{
    public class LanguageDeleted : DomainEvent
    {
        public Guid SiteId { get; set; }
    }
}