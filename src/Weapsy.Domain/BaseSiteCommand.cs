using System;
using Weapsy.Cqrs.Domain;

namespace Weapsy.Domain
{
    public class BaseSiteCommand : DomainCommand
    {
        public Guid SiteId { get; set; }
    }
}
