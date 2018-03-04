using FluentValidation;
using Weapsy.Cqrs.Commands;
using Weapsy.Cqrs.Domain;
using Weapsy.Domain.Pages.Commands;

namespace Weapsy.Domain.Pages.Handlers
{
    public class CreatePageHandler : ICommandHandlerWithAggregate<CreatePage>
    {
        private readonly IPageRepository _pageRepository;
        private readonly IValidator<CreatePage> _validator;

        public CreatePageHandler(IPageRepository pageRepository,
            IValidator<CreatePage> validator)
        {
            _pageRepository = pageRepository;
            _validator = validator;
        }

        public IAggregateRoot Handle(CreatePage command)
        {
            var page = Page.CreateNew(command, _validator);

            _pageRepository.Create(page);

            return page;
        }
    }
}
