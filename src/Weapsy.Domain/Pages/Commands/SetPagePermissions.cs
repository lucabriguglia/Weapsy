using System;
using System.Collections.Generic;

namespace Weapsy.Domain.Pages.Commands
{
    public class SetPagePermissions : BaseSiteCommand
    {
        public Guid Id { get; set; }
        public IList<PagePermission> PagePermissions { get; set; }
    }
}
