using System;
using System.Collections.Generic;
using FluentValidation;
using Weapsy.Framework.Domain;
using Weapsy.Domain.Modules;
using Weapsy.Domain.Modules.Commands;
using Weapsy.Domain.Pages.Commands;
using Weapsy.Framework.Commands;
using Weapsy.Framework.Events;

//using System.Transactions;

namespace Weapsy.Domain.Pages.Handlers
{
    public class RemoveModuleHandler : ICommandHandler<RemoveModule>
    {
        private readonly IModuleRepository _moduleRepository;
        private readonly IPageRepository _pageRepository;
        private readonly IValidator<DeleteModule> _deleteModuleValidator;
        private readonly IValidator<RemovePageModule> _removePageModuleValidator;

        public RemoveModuleHandler(IModuleRepository moduleRepository, 
            IPageRepository pageRepository, 
            IValidator<DeleteModule> deleteModuleValidator,
            IValidator<RemovePageModule> removePageModuleValidator)
        {
            _moduleRepository = moduleRepository;
            _pageRepository = pageRepository;
            _deleteModuleValidator = deleteModuleValidator;
            _removePageModuleValidator = removePageModuleValidator;
        }

        public IEnumerable<IEvent> Handle(RemoveModule cmd)
        {
            var events = new List<IDomainEvent>();

            //using (var scope = new TransactionScope(TransactionScopeOption.Suppress))
            //{
                var page = _pageRepository.GetById(cmd.SiteId, cmd.PageId);
                if (page == null)
                    throw new Exception("Page not found");

                var module = _moduleRepository.GetById(cmd.SiteId, cmd.ModuleId);
                if (module == null)
                    throw new Exception("Module not found");

                page.RemoveModule(new RemovePageModule
                {
                    SiteId = cmd.SiteId,
                    PageId = cmd.PageId,
                    ModuleId = cmd.ModuleId
                }, _removePageModuleValidator);
                events.AddRange(page.Events);
                _pageRepository.Update(page);

                if (_moduleRepository.GetCountByModuleId(cmd.ModuleId) == 1)
                {
                    module.Delete();
                    events.AddRange(module.Events);
                    _moduleRepository.Update(module);
                }

            //    scope.Complete();
            //}

            return events;
        }
    }
}