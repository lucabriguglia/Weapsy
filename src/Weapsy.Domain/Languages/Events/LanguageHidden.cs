using System;
using Weapsy.Infrastructure.Domain;

namespace Weapsy.Domain.Languages.Events
{
    public class LanguageHidden : Event
    {
        public Guid SiteId { get; set; }
    }
}