using System.Collections.Generic;
using FluentValidation;
using Weapsy.Domain.Pages.Commands;
using Weapsy.Framework.Commands;
using Weapsy.Framework.Events;

namespace Weapsy.Domain.Pages.Handlers
{
    public class CreatePageHandler : ICommandHandler<CreatePage>
    {
        private readonly IPageRepository _pageRepository;
        private readonly IValidator<CreatePage> _validator;

        public CreatePageHandler(IPageRepository pageRepository,
            IValidator<CreatePage> validator)
        {
            _pageRepository = pageRepository;
            _validator = validator;
        }

        public IEnumerable<IEvent> Handle(CreatePage command)
        {
            var page = Page.CreateNew(command, _validator);

            _pageRepository.Create(page);

            return page.Events;
        }
    }
}
