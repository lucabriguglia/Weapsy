using System;
using Weapsy.Infrastructure.Dispatcher;
using Weapsy.Infrastructure.Domain;

namespace Weapsy.Domain
{
    public class BaseSiteCommand : ICommand
    {
        public Guid SiteId { get; set; }
    }
}
