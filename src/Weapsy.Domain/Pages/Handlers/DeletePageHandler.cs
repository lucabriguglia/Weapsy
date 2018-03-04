using FluentValidation;
using System;
using Weapsy.Cqrs.Commands;
using Weapsy.Cqrs.Domain;
using Weapsy.Domain.Pages.Commands;

namespace Weapsy.Domain.Pages.Handlers
{
    public class DeletePageHandler : ICommandHandlerWithAggregate<DeletePage>
    {
        private readonly IPageRepository _pageRepository;
        private readonly IValidator<DeletePage> _validator;

        public DeletePageHandler(IPageRepository pageRepository, IValidator<DeletePage> validator)
        {
            _pageRepository = pageRepository;
            _validator = validator;
        }

        public IAggregateRoot Handle(DeletePage command)
        {
            var page = _pageRepository.GetById(command.SiteId, command.Id);

            if (page == null)
                throw new Exception("Page not found.");

            page.Delete(command, _validator);

            _pageRepository.Update(page);

            return page;
        }
    }
}
