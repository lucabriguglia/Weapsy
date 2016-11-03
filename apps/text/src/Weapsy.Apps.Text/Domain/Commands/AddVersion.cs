using System;
using System.Collections.Generic;
using Weapsy.Infrastructure.Domain;

namespace Weapsy.Apps.Text.Domain.Commands
{
    public class AddVersion : ICommand
    {
        public Guid SiteId { get; set; }
        public Guid ModuleId { get; set; }
        public Guid Id { get; set; }
        public Guid VersionId { get; set; }
        public string Content { get; set; }
        public string Description { get; set; }
        public TextVersionStatus Status { get; set; }
        public IList<VersionLocalisation> VersionLocalisations { get; set; } = new List<VersionLocalisation>();

        public class VersionLocalisation
        {
            public Guid LanguageId { get; set; }
            public string Content { get; set; }
        }
    }
}
