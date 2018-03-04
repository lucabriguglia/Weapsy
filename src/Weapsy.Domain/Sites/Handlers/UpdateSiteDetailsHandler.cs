using FluentValidation;
using Weapsy.Domain.Sites.Commands;
using System;
using Weapsy.Cqrs.Commands;
using Weapsy.Cqrs.Domain;

namespace Weapsy.Domain.Sites.Handlers
{
    public class UpdateSiteDetailsHandler : ICommandHandlerWithAggregate<UpdateSiteDetails>
    {
        private readonly ISiteRepository _siteRepository;
        private readonly IValidator<UpdateSiteDetails> _validator;

        public UpdateSiteDetailsHandler(ISiteRepository siteRepository,
            IValidator<UpdateSiteDetails> validator)
        {
            _siteRepository = siteRepository;
            _validator = validator;
        }

        public IAggregateRoot Handle(UpdateSiteDetails cmd)
        {
            var site = _siteRepository.GetById(cmd.SiteId);

            if (site == null)
                throw new Exception("Site not found");

            site.UpdateDetails(cmd, _validator);

            _siteRepository.Update(site);

            return site;
        }
    }
}
