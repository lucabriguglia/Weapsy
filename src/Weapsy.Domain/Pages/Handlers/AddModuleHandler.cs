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
    public class AddModuleHandler : ICommandHandler<AddModule>
    {
        private readonly IModuleRepository _moduleRepository;
        private readonly IPageRepository _pageRepository;
        private readonly IValidator<CreateModule> _createModuleValidator;
        private readonly IValidator<AddPageModule> _addPageModuleValidator;

        public AddModuleHandler(IModuleRepository moduleRepository,
            IPageRepository pageRepository,
            IValidator<CreateModule> createModuleValidator,
            IValidator<AddPageModule> addPageModuleValidator)
        {
            _moduleRepository = moduleRepository;
            _pageRepository = pageRepository;
            _createModuleValidator = createModuleValidator;
            _addPageModuleValidator = addPageModuleValidator;
        }

        public IEnumerable<IEvent> Handle(AddModule cmd)
        {
            var events = new List<IDomainEvent>();

            //using (var scope = new TransactionScope(TransactionScopeOption.Suppress))
            //{

                var page = _pageRepository.GetById(cmd.SiteId, cmd.PageId);

                if (page == null)
                    throw new Exception("Page not found");

                var moduleId = Guid.NewGuid();

                var module = Module.CreateNew(new CreateModule
                {
                    SiteId = cmd.SiteId,
                    ModuleTypeId = cmd.ModuleTypeId,
                    Id = moduleId,
                    Title = cmd.Title
                }, _createModuleValidator);
                _moduleRepository.Create(module);
                events.AddRange(module.Events);

                page.AddModule(new AddPageModule
                {
                    SiteId = cmd.SiteId,
                    PageId = cmd.PageId,
                    ModuleId = moduleId,
                    PageModuleId = Guid.NewGuid(),
                    Title = cmd.Title,
                    Zone = cmd.Zone,
                    SortOrder = cmd.SortOrder,
                    PageModulePermissions = cmd.PageModulePermissions
                }, _addPageModuleValidator);
                events.AddRange(page.Events);
                _pageRepository.Update(page);

            //    scope.Complete();
            //}

            return events;
        }
    }
}