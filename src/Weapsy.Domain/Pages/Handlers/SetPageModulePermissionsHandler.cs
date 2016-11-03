using System;
using System.Collections.Generic;
using Weapsy.Infrastructure.Domain;
using Weapsy.Domain.Pages.Commands;

namespace Weapsy.Domain.Pages.Handlers
{
    public class SetPageModulePermissionsHandler : ICommandHandler<SetPageModulePermissions>
    {
        private readonly IPageRepository _pageRepository;

        public SetPageModulePermissionsHandler(IPageRepository pageRepository)
        {
            _pageRepository = pageRepository;
        }

        public ICollection<IEvent> Handle(SetPageModulePermissions command)
        {
            var page = _pageRepository.GetById(command.SiteId, command.Id);

            if (page == null)
                throw new Exception("Page not found.");

            page.SetModulePermissions(command);

            _pageRepository.Update(page);

            return page.Events;
        }
    }
}
