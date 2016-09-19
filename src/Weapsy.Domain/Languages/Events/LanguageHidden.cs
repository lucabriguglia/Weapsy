using System;
using Weapsy.Core.Domain;

namespace Weapsy.Domain.Model.Languages.Events
{
    public class LanguageHidden : Event
    {
        public Guid SiteId { get; set; }
    }
}