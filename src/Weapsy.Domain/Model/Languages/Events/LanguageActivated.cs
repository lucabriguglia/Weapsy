using System;
using Weapsy.Core.Domain;

namespace Weapsy.Domain.Model.Languages.Events
{
    public class LanguageActivated : Event
    {
        public Guid SiteId { get; set; }
    }
}