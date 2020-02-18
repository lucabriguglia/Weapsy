using System;

namespace Weapsy.Domain.Models.Sites.Commands
{
    public class UpdateSite : SiteCommandBase
    {
        public Guid Id { get; set; }
    }
}
