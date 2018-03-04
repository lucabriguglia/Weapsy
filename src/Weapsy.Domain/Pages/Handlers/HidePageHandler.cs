using FluentValidation;
using System;
using Weapsy.Cqrs.Commands;
using Weapsy.Cqrs.Domain;
using Weapsy.Domain.Pages.Commands;

namespace Weapsy.Domain.Pages.Handlers
{
    public class HidePageHandler : ICommandHandlerWithAggregate<HidePage>
    {
        private readonly IPageRepository _pageRepository;
        private readonly IValidator<HidePage> _validator;

        public HidePageHandler(IPageRepository pageRepository, IValidator<HidePage> validator)
        {
            _pageRepository = pageRepository;
            _validator = validator;
        }

        public IAggregateRoot Handle(HidePage command)
        {
            var page = _pageRepository.GetById(command.SiteId, command.Id);

            if (page == null)
                throw new Exception("Page not found.");

            page.Hide(command, _validator);

            _pageRepository.Update(page);

            return page;
        }
    }
}
