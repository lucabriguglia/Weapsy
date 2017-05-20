using System;
using System.Collections.Generic;
using Weapsy.Domain.Sites.Commands;
using Weapsy.Framework.Commands;
using Weapsy.Framework.Events;

namespace Weapsy.Domain.Sites.Handlers
{
    public class CloseSiteHandler : ICommandHandler<CloseSite>
    {
        private readonly ISiteRepository _siteRepository;

        public CloseSiteHandler(ISiteRepository siteRepository)
        {
            _siteRepository = siteRepository;
        }

        public IEnumerable<IEvent> Handle(CloseSite command)
        {
            var site = _siteRepository.GetById(command.Id);

            if (site == null)
                throw new Exception("Site not found.");

            site.Close();

            _siteRepository.Update(site);

            return site.Events;
        }
    }
}
