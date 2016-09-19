using System;
using System.Collections.Generic;
using Weapsy.Core.Domain;
using Weapsy.Domain.Model.Pages.Commands;

namespace Weapsy.Domain.Model.Pages.Handlers
{
    public class SetPagePermissionsHandler : ICommandHandler<SetPagePermissions>
    {
        private readonly IPageRepository _pageRepository;

        public SetPagePermissionsHandler(IPageRepository pageRepository)
        {
            _pageRepository = pageRepository;
        }

        public ICollection<IEvent> Handle(SetPagePermissions command)
        {
            var page = _pageRepository.GetById(command.SiteId, command.Id);

            if (page == null)
                throw new Exception("Page not found.");

            page.SetPagePermissions(command);

            _pageRepository.Update(page);

            return page.Events;
        }
    }
}
