using System.Collections.Generic;
using FluentValidation;
using Weapsy.Domain.Sites.Commands;
using System;
using Weapsy.Framework.Commands;
using Weapsy.Framework.Events;

namespace Weapsy.Domain.Sites.Handlers
{
    public class UpdateSiteDetailsHandler : ICommandHandler<UpdateSiteDetailsCommand>
    {
        private readonly ISiteRepository _siteRepository;
        private readonly IValidator<UpdateSiteDetailsCommand> _validator;

        public UpdateSiteDetailsHandler(ISiteRepository siteRepository,
            IValidator<UpdateSiteDetailsCommand> validator)
        {
            _siteRepository = siteRepository;
            _validator = validator;
        }

        public IEnumerable<IEvent> Handle(UpdateSiteDetailsCommand cmd)
        {
            var site = _siteRepository.GetById(cmd.SiteId);

            if (site == null)
                throw new Exception("Site not found");

            site.UpdateDetails(cmd, _validator);

            _siteRepository.Update(site);

            return site.Events;
        }
    }
}
