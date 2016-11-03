using System.Collections.Generic;
using FluentValidation;
using Weapsy.Infrastructure.Domain;
using Weapsy.Domain.Pages.Commands;
using System;

namespace Weapsy.Domain.Pages.Handlers
{
    public class UpdatePageDetailsHandler : ICommandHandler<UpdatePageDetails>
    {
        private readonly IPageRepository _pageRepository;
        private readonly IValidator<UpdatePageDetails> _validator;

        public UpdatePageDetailsHandler(IPageRepository pageRepository,
            IValidator<UpdatePageDetails> validator)
        {
            _pageRepository = pageRepository;
            _validator = validator;
        }

        public ICollection<IEvent> Handle(UpdatePageDetails command)
        {
            var page = _pageRepository.GetById(command.SiteId, command.Id);

            if (page == null)
                throw new Exception("Page not found");

            page.UpdateDetails(command, _validator);

            _pageRepository.Update(page);

            return page.Events;
        }
    }
}
