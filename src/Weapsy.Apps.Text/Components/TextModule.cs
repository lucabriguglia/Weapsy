using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Weapsy.Mvc.Components;
using Weapsy.Mvc.Context;
using Weapsy.Reporting.Pages;
using Weapsy.Apps.Text.Reporting;
using Weapsy.Apps.Text.Domain.Commands;
using System;
using Weapsy.Apps.Text.Domain;
using Weapsy.Framework.Commands;
using Weapsy.Framework.Queries;

namespace Weapsy.Apps.Text.Components
{
    [ViewComponent(Name = "TextModule")]
    public class TextViewComponent : BaseViewComponent
    {
        private readonly IContextService _contextService;
        private readonly ICommandSender _commandSender;
        private readonly IQueryDispatcher _queryDispatcher;

        public TextViewComponent(IContextService contextService, 
            ICommandSender commandSender, 
            IQueryDispatcher queryDispatcher)
            : base(contextService)
        {
            _contextService = contextService;
            _commandSender = commandSender;
            _queryDispatcher = queryDispatcher;
        }

        public async Task<IViewComponentResult> InvokeAsync(ModuleModel model)
        {
            var content = await _queryDispatcher.DispatchAsync<GetContent, string>(new GetContent
            {
                ModuleId = model.Id,
                LanguageId = _contextService.GetCurrentLanguageInfo().Id
            });

            if (content == null)
            {
                var defaultContent = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";

                _commandSender.Send<CreateTextModule, TextModule>(new CreateTextModule
                {
                    SiteId = SiteId,
                    Id = Guid.NewGuid(),
                    ModuleId = model.Id,
                    Content = defaultContent
                });

                content = defaultContent;
            }

            return View("~/Apps/Weapsy.Apps.Text/Views/Shared/Components/TextModule/Default.cshtml", content);
        }
    }
}
