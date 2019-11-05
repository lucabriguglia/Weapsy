using Kledex.Domain;
using Weapsy.Domain.Models.Sites.Commands;
using Weapsy.Domain.Models.Sites.Events;

namespace Weapsy.Domain.Models.Sites
{
    public class Site : AggregateRoot
    {
        public string Name { get; private set; }

        public Site()
        {
        }

        public Site(CreateSite command)
        {
            AddAndApplyEvent(new SiteCreated
            {
                AggregateRootId = command.AggregateRootId,
                Name = command.Name
            });
        }

        private void Apply(SiteCreated @event)
        {
            Id = @event.AggregateRootId;
            Name = @event.Name;
        }
    }
}
