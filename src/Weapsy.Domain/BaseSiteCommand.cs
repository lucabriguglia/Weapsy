using System;
using Weapsy.Infrastructure.Dispatcher;

namespace Weapsy.Domain
{
    public class BaseSiteCommand : ICommand
    {
        public Guid SiteId { get; set; }
    }
}
