using FluentValidation;
using System;
using System.Collections.Generic;
using Weapsy.Domain.Pages.Commands;
using Weapsy.Framework.Commands;
using Weapsy.Framework.Events;

namespace Weapsy.Domain.Pages.Handlers
{
    public class HidePageHandler : ICommandHandler<HidePageCommand>
    {
        private readonly IPageRepository _pageRepository;
        private readonly IValidator<HidePageCommand> _validator;

        public HidePageHandler(IPageRepository pageRepository, IValidator<HidePageCommand> validator)
        {
            _pageRepository = pageRepository;
            _validator = validator;
        }

        public IEnumerable<IEvent> Handle(HidePageCommand command)
        {
            var page = _pageRepository.GetById(command.SiteId, command.Id);

            if (page == null)
                throw new Exception("Page not found.");

            page.Hide(command, _validator);

            _pageRepository.Update(page);

            return page.Events;
        }
    }
}
