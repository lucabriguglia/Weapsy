using System;
using Weapsy.Core.Domain;

namespace Weapsy.Domain.Languages.Events
{
    public class LanguageHidden : Event
    {
        public Guid SiteId { get; set; }
    }
}