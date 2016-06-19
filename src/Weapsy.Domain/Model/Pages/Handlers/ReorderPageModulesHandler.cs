using System.Collections.Generic;
using Weapsy.Core.Domain;
using Weapsy.Domain.Model.Pages.Commands;
using System;
using FluentValidation;

namespace Weapsy.Domain.Model.Pages.Handlers
{
    public class ReorderPageModulesHandler : ICommandHandler<ReorderPageModules>
    {
        private readonly IPageRepository _pageRepository;
        private readonly IValidator<ReorderPageModules> _validator;

        public ReorderPageModulesHandler(IPageRepository pageRepository, IValidator<ReorderPageModules> validator)
        {
            _pageRepository = pageRepository;
            _validator = validator;
        }

        public ICollection<IEvent> Handle(ReorderPageModules cmd)
        {
            var page = _pageRepository.GetById(cmd.SiteId, cmd.PageId);

            if (page == null)
                throw new Exception("Page not found");

            page.ReorderPageModules(cmd, _validator);

            _pageRepository.Update(page);

            return page.Events;
        }
    }
}
