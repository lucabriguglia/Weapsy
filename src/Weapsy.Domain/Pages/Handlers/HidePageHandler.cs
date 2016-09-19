using FluentValidation;
using System;
using System.Collections.Generic;
using Weapsy.Core.Domain;
using Weapsy.Domain.Pages.Commands;

namespace Weapsy.Domain.Pages.Handlers
{
    public class HidePageHandler : ICommandHandler<HidePage>
    {
        private readonly IPageRepository _pageRepository;
        private readonly IValidator<HidePage> _validator;

        public HidePageHandler(IPageRepository pageRepository, IValidator<HidePage> validator)
        {
            _pageRepository = pageRepository;
            _validator = validator;
        }

        public ICollection<IEvent> Handle(HidePage command)
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
