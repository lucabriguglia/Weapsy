using System;
using System.Collections.Generic;
using Weapsy.Apps.Text.Domain.Commands;
using Weapsy.Cqrs.Domain;

namespace Weapsy.Apps.Text.Domain.Events
{
    public class VersionAdded : DomainEvent
    {
        public Guid SiteId { get; set; }
        public Guid ModuleId { get; set; }
        public Guid VersionId { get; set; }
        public string Content { get; set; }
        public string Description { get; set; }
        public TextVersionStatus Status { get; set; }

        public IList<AddVersion.VersionLocalisation> VersionLocalisations { get; set; } = new List<AddVersion.VersionLocalisation>();

        public class VersionLocalisation
        {
            public Guid LanguageId { get; set; }
            public string LanguageName { get; set; }
            public string Content { get; set; }
        }
    }
}
