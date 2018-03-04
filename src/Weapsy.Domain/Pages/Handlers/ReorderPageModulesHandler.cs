using Weapsy.Domain.Pages.Commands;
using System;
using FluentValidation;
using Weapsy.Cqrs.Commands;
using Weapsy.Cqrs.Domain;

namespace Weapsy.Domain.Pages.Handlers
{
    public class ReorderPageModulesHandler : ICommandHandlerWithAggregate<ReorderPageModules>
    {
        private readonly IPageRepository _pageRepository;
        private readonly IValidator<ReorderPageModules> _validator;

        public ReorderPageModulesHandler(IPageRepository pageRepository, IValidator<ReorderPageModules> validator)
        {
            _pageRepository = pageRepository;
            _validator = validator;
        }

        public IAggregateRoot Handle(ReorderPageModules cmd)
        {
            var page = _pageRepository.GetById(cmd.SiteId, cmd.PageId);

            if (page == null)
                throw new Exception("Page not found");

            page.ReorderPageModules(cmd, _validator);

            _pageRepository.Update(page);

            return page;
        }
    }
}
