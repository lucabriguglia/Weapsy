using System;
using Weapsy.Cqrs.Commands;
using Weapsy.Cqrs.Domain;
using Weapsy.Domain.Pages.Commands;

namespace Weapsy.Domain.Pages.Handlers
{
    public class SetPageModulePermissionsHandler : ICommandHandlerWithAggregate<SetPageModulePermissions>
    {
        private readonly IPageRepository _pageRepository;

        public SetPageModulePermissionsHandler(IPageRepository pageRepository)
        {
            _pageRepository = pageRepository;
        }

        public IAggregateRoot Handle(SetPageModulePermissions command)
        {
            var page = _pageRepository.GetById(command.SiteId, command.Id);

            if (page == null)
                throw new Exception("Page not found.");

            page.SetModulePermissions(command);

            _pageRepository.Update(page);

            return page;
        }
    }
}
