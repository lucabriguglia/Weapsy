using System;
using Weapsy.Core.Domain;

namespace Weapsy.Domain.Languages.Events
{
    public class LanguageActivated : Event
    {
        public Guid SiteId { get; set; }
    }
}