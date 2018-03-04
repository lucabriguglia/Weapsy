using System;
using FluentValidation;
using Weapsy.Cqrs.Commands;
using Weapsy.Cqrs.Domain;
using Weapsy.Domain.Pages.Commands;

namespace Weapsy.Domain.Pages.Handlers
{
    public class AddPageModuleHandler : ICommandHandlerWithAggregate<AddPageModule>
    {
        private readonly IPageRepository _pageRepository;
        private readonly IValidator<AddPageModule> _addPageModuleValidator;

        public AddPageModuleHandler(IPageRepository pageRepository,
            IValidator<AddPageModule> addPageModuleValidator)
        {
            _pageRepository = pageRepository;
            _addPageModuleValidator = addPageModuleValidator;
        }

        public IAggregateRoot Handle(AddPageModule cmd)
        {
            var page = _pageRepository.GetById(cmd.SiteId, cmd.PageId);

            if (page == null)
                throw new Exception("Page not found");

            page.AddModule(cmd, _addPageModuleValidator);

            _pageRepository.Update(page);

            return page;
        }
    }
}