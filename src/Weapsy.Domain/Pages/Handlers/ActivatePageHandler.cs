using FluentValidation;
using System;
using Weapsy.Cqrs.Commands;
using Weapsy.Cqrs.Domain;
using Weapsy.Domain.Pages.Commands;

namespace Weapsy.Domain.Pages.Handlers
{
    public class ActivatePageHandler : ICommandHandlerWithAggregate<ActivatePage>
    {
        private readonly IPageRepository _pageRepository;
        private readonly IValidator<ActivatePage> _validator;

        public ActivatePageHandler(IPageRepository pageRepository, IValidator<ActivatePage> validator)
        {
            _pageRepository = pageRepository;
            _validator = validator;
        }

        public IAggregateRoot Handle(ActivatePage command)
        {
            var page = _pageRepository.GetById(command.SiteId, command.Id);

            if (page == null)
                throw new Exception("Page not found.");

            page.Activate(command, _validator);

            _pageRepository.Update(page);

            return page;
        }
    }
}
