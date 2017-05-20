using FluentValidation;
using System;
using System.Collections.Generic;
using Weapsy.Domain.Pages.Commands;
using Weapsy.Framework.Commands;
using Weapsy.Framework.Events;

namespace Weapsy.Domain.Pages.Handlers
{
    public class ActivatePageHandler : ICommandHandler<ActivatePage>
    {
        private readonly IPageRepository _pageRepository;
        private readonly IValidator<ActivatePage> _validator;

        public ActivatePageHandler(IPageRepository pageRepository, IValidator<ActivatePage> validator)
        {
            _pageRepository = pageRepository;
            _validator = validator;
        }

        public IEnumerable<IEvent> Handle(ActivatePage command)
        {
            var page = _pageRepository.GetById(command.SiteId, command.Id);

            if (page == null)
                throw new Exception("Page not found.");

            page.Activate(command, _validator);

            _pageRepository.Update(page);

            return page.Events;
        }
    }
}
