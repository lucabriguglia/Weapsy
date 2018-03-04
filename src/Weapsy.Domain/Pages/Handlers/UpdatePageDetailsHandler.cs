using FluentValidation;
using Weapsy.Domain.Pages.Commands;
using System;
using Weapsy.Cqrs.Commands;
using Weapsy.Cqrs.Domain;

namespace Weapsy.Domain.Pages.Handlers
{
    public class UpdatePageDetailsHandler : ICommandHandlerWithAggregate<UpdatePageDetails>
    {
        private readonly IPageRepository _pageRepository;
        private readonly IValidator<UpdatePageDetails> _validator;

        public UpdatePageDetailsHandler(IPageRepository pageRepository,
            IValidator<UpdatePageDetails> validator)
        {
            _pageRepository = pageRepository;
            _validator = validator;
        }

        public IAggregateRoot Handle(UpdatePageDetails command)
        {
            var page = _pageRepository.GetById(command.SiteId, command.Id);

            if (page == null)
                throw new Exception("Page not found");

            page.UpdateDetails(command, _validator);

            _pageRepository.Update(page);

            return page;
        }
    }
}
