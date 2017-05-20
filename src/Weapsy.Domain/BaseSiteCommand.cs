using System;
using Weapsy.Framework.Commands;

namespace Weapsy.Domain
{
    public class BaseSiteCommand : ICommand
    {
        public Guid SiteId { get; set; }
    }
}
