using System;
using System.Collections.Generic;
using Weapsy.Apps.Text.Domain;

namespace Weapsy.Apps.Text.Reporting
{
    public class TextModuleAdminModel
    {
        public Guid Id { get; set; }
        public Guid ModuleId { get; set; }
        public string Content { get; set; }
        public string Description { get; set; }
        public TextVersionStatus Status { get; set; }

        public IList<VersionLocalisation> VersionLocalisations { get; set; } = new List<VersionLocalisation>();

        public class VersionLocalisation
        {
            public Guid LanguageId { get; set; }
            public string LanguageName { get; set; }
            public string Content { get; set; }
        }
    }
}
