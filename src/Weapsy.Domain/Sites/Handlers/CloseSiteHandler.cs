using System;
using Weapsy.Cqrs.Commands;
using Weapsy.Cqrs.Domain;
using Weapsy.Domain.Sites.Commands;

namespace Weapsy.Domain.Sites.Handlers
{
    public class CloseSiteHandler : ICommandHandlerWithAggregate<CloseSite>
    {
        private readonly ISiteRepository _siteRepository;

        public CloseSiteHandler(ISiteRepository siteRepository)
        {
            _siteRepository = siteRepository;
        }

        public IAggregateRoot Handle(CloseSite command)
        {
            var site = _siteRepository.GetById(command.Id);

            if (site == null)
                throw new Exception("Site not found.");

            site.Close();

            _siteRepository.Update(site);

            return site;
        }
    }
}
