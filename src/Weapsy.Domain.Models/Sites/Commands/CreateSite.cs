using System;

namespace Weapsy.Domain.Models.Sites.Commands
{
    public class CreateSite : SiteCommandBase
    {
        public CreateSite()
        {
            SiteId = Guid.NewGuid();
        }
    }
}
