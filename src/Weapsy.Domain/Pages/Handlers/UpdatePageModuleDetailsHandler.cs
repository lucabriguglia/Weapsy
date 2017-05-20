using System.Collections.Generic;
using FluentValidation;
using Weapsy.Domain.Pages.Commands;
using System;
using Weapsy.Framework.Commands;
using Weapsy.Framework.Events;

namespace Weapsy.Domain.Pages.Handlers
{
    public class UpdatePageModuleDetailsHandler : ICommandHandler<UpdatePageModuleDetails>
    {
        private readonly IPageRepository _pageRepository;
        private readonly IValidator<UpdatePageModuleDetails> _validator;

        public UpdatePageModuleDetailsHandler(IPageRepository pageRepository,
            IValidator<UpdatePageModuleDetails> validator)
        {
            _pageRepository = pageRepository;
            _validator = validator;
        }

        public IEnumerable<IEvent> Handle(UpdatePageModuleDetails command)
        {
            var page = _pageRepository.GetById(command.SiteId, command.PageId);

            if (page == null)
                throw new Exception("Page not found");

            page.UpdateModule(command, _validator);

            _pageRepository.Update(page);

            return page.Events;
        }
    }
}
