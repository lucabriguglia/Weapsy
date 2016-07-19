using System;
using System.Collections.Generic;
using Weapsy.Core.Domain;

namespace Weapsy.Domain.Model.Pages.Commands
{
    public class SetPagePermissions : ICommand
    {
        public Guid SiteId { get; set; }
        public Guid Id { get; set; }
        public IList<PagePermission> PagePermissions { get; set; }
    }
}
