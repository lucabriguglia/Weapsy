using System;
using Weapsy.Cqrs.Commands;
using Weapsy.Cqrs.Domain;
using Weapsy.Domain.Sites.Commands;

namespace Weapsy.Domain.Sites.Handlers
{
    public class DeleteSiteHandler : ICommandHandlerWithAggregate<DeleteSite>
    {
        private readonly ISiteRepository _siteRepository;

        public DeleteSiteHandler(ISiteRepository siteRepository)
        {
            _siteRepository = siteRepository;
        }

        public IAggregateRoot Handle(DeleteSite command)
        {
            var site = _siteRepository.GetById(command.Id);

            if (site == null)
                throw new Exception("Site not found.");

            site.Delete();

            _siteRepository.Update(site);

            return site;
        }
    }
}
