using System;

namespace Weapsy.Apps.Text.Domain.Commands
{
    public class PublishVersion
    {
        public Guid SiteId { get; set; }
        public Guid ModuleId { get; set; }
        public Guid Id { get; set; }
        public Guid VersionId { get; set; }
    }
}
