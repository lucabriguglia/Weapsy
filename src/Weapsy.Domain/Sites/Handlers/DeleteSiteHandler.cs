using System;
using System.Collections.Generic;
using Weapsy.Core.Domain;
using Weapsy.Domain.Model.Sites.Commands;

namespace Weapsy.Domain.Model.Sites.Handlers
{
    public class DeleteSiteHandler : ICommandHandler<DeleteSite>
    {
        private readonly ISiteRepository _siteRepository;

        public DeleteSiteHandler(ISiteRepository siteRepository)
        {
            _siteRepository = siteRepository;
        }

        public ICollection<IEvent> Handle(DeleteSite command)
        {
            var site = _siteRepository.GetById(command.Id);

            if (site == null)
                throw new Exception("Site not found.");

            site.Delete();

            _siteRepository.Update(site);

            return site.Events;
        }
    }
}
