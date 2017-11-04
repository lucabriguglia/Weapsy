using System.Collections.Generic;
using FluentValidation;
using Weapsy.Domain.Pages.Commands;
using System;
using Weapsy.Framework.Commands;
using Weapsy.Framework.Events;

namespace Weapsy.Domain.Pages.Handlers
{
    public class UpdatePageDetailsHandler : ICommandHandler<UpdatePageDetailsCommand>
    {
        private readonly IPageRepository _pageRepository;
        private readonly IValidator<UpdatePageDetailsCommand> _validator;

        public UpdatePageDetailsHandler(IPageRepository pageRepository,
            IValidator<UpdatePageDetailsCommand> validator)
        {
            _pageRepository = pageRepository;
            _validator = validator;
        }

        public IEnumerable<IEvent> Handle(UpdatePageDetailsCommand command)
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
