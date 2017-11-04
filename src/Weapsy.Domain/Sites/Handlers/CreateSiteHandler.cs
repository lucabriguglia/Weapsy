using System.Collections.Generic;
using FluentValidation;
using Weapsy.Domain.Sites.Commands;
using Weapsy.Framework.Commands;
using Weapsy.Framework.Events;

namespace Weapsy.Domain.Sites.Handlers
{
    public class CreateSiteHandler : ICommandHandler<CreateSiteCommand>
    {
        private readonly ISiteRepository _siteRepository;
        private readonly IValidator<CreateSiteCommand> _validator;

        public CreateSiteHandler(ISiteRepository siteRepository, IValidator<CreateSiteCommand> validator)
        {
            _siteRepository = siteRepository;
            _validator = validator;
        }

        public IEnumerable<IEvent> Handle(CreateSiteCommand command)
        {
            var site = Site.CreateNew(command, _validator);

            _siteRepository.Create(site);

            return site.Events;
        }
    }
}
