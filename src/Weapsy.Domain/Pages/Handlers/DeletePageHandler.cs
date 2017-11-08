using FluentValidation;
using System;
using System.Collections.Generic;
using Weapsy.Domain.Pages.Commands;
using Weapsy.Framework.Commands;
using Weapsy.Framework.Events;

namespace Weapsy.Domain.Pages.Handlers
{
    public class DeletePageHandler : ICommandHandler<DeletePageCommand>
    {
        private readonly IPageRepository _pageRepository;
        private readonly IValidator<DeletePageCommand> _validator;

        public DeletePageHandler(IPageRepository pageRepository, IValidator<DeletePageCommand> validator)
        {
            _pageRepository = pageRepository;
            _validator = validator;
        }

        public IEnumerable<IEvent> Handle(DeletePageCommand command)
        {
            var page = _pageRepository.GetById(command.SiteId, command.Id);

            if (page == null)
                throw new Exception("Page not found.");

            page.Delete(command, _validator);

            _pageRepository.Update(page);

            return page.Events;
        }
    }
}
