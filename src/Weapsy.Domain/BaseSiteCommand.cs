using System;
using Weapsy.Infrastructure.Commands;

namespace Weapsy.Domain
{
    public class BaseSiteCommand : ICommand
    {
        public Guid SiteId { get; set; }
    }
}
