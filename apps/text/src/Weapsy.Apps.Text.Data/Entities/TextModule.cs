using System;
using System.Collections.Generic;
using Weapsy.Apps.Text.Domain;

namespace Weapsy.Apps.Text.Data.Entities
{
    public class TextModule
    {
        public Guid Id { get; set; }
        public Guid ModuleId { get; set; }
        public TextModuleStatus Status { get; set; }

        public IList<TextVersion> TextVersions { get; set; } = new List<TextVersion>();
    }
}
