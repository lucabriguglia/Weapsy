using FluentValidation;
using Weapsy.Domain.Pages.Commands;
using System;
using Weapsy.Cqrs.Commands;
using Weapsy.Cqrs.Domain;

namespace Weapsy.Domain.Pages.Handlers
{
    public class UpdatePageModuleDetailsHandler : ICommandHandlerWithAggregate<UpdatePageModuleDetails>
    {
        private readonly IPageRepository _pageRepository;
        private readonly IValidator<UpdatePageModuleDetails> _validator;

        public UpdatePageModuleDetailsHandler(IPageRepository pageRepository,
            IValidator<UpdatePageModuleDetails> validator)
        {
            _pageRepository = pageRepository;
            _validator = validator;
        }

        public IAggregateRoot Handle(UpdatePageModuleDetails command)
        {
            var page = _pageRepository.GetById(command.SiteId, command.PageId);

            if (page == null)
                throw new Exception("Page not found");

            page.UpdateModule(command, _validator);

            _pageRepository.Update(page);

            return page;
        }
    }
}
