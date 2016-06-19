using System.Collections.Generic;
using Weapsy.Core.Domain;
using Weapsy.Domain.Model.Modules;
using Weapsy.Domain.Model.Pages;
using Weapsy.Domain.Services.Modules.Commands;
//using System.Transactions;
using System;
using FluentValidation;
using Weapsy.Domain.Model.Modules.Commands;
using Weapsy.Domain.Model.Pages.Commands;

namespace Weapsy.Domain.Services.Modules.Handlers
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

        public ICollection<IEvent> Handle(RemoveModule cmd)
        {
            var events = new List<IEvent>();

            //using (var scope = new TransactionScope(TransactionScopeOption.Suppress))
            //{
                var page = _pageRepository.GetById(cmd.SiteId, cmd.PageId);
                if (page == null) throw new Exception("Page not found");

                var module = _moduleRepository.GetById(cmd.SiteId, cmd.ModuleId);
                if (module == null) throw new Exception("Module not found");

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