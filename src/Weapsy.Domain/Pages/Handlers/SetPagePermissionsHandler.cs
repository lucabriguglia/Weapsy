using System;
using Weapsy.Cqrs.Commands;
using Weapsy.Cqrs.Domain;
using Weapsy.Domain.Pages.Commands;

namespace Weapsy.Domain.Pages.Handlers
{
    public class SetPagePermissionsHandler : ICommandHandlerWithAggregate<SetPagePermissions>
    {
        private readonly IPageRepository _pageRepository;

        public SetPagePermissionsHandler(IPageRepository pageRepository)
        {
            _pageRepository = pageRepository;
        }

        public IAggregateRoot Handle(SetPagePermissions command)
        {
            var page = _pageRepository.GetById(command.SiteId, command.Id);

            if (page == null)
                throw new Exception("Page not found.");

            page.SetPagePermissions(command);

            _pageRepository.Update(page);

            return page;
        }
    }
}
