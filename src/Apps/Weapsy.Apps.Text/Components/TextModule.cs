using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Weapsy.Mvc.Components;
using Weapsy.Mvc.Context;
using Weapsy.Reporting.Pages;
using Weapsy.Apps.Text.Reporting;
using Weapsy.Infrastructure.Dispatcher;
using Weapsy.Apps.Text.Domain.Commands;
using System;
using Weapsy.Apps.Text.Domain;

namespace Weapsy.Apps.Text.Components
{
    [ViewComponent(Name = "TextModule")]
    public class TextViewComponent : BaseViewComponent
    {
        private readonly IContextService _contextService;
        private readonly ITextModuleFacade _textModuleFacade;
        private readonly ICommandSender _commandSender;

        public TextViewComponent(IContextService contextService, 
            ITextModuleFacade textFacade,
            ICommandSender commandSender)
            : base(contextService)
        {
            _contextService = contextService;
            _textModuleFacade = textFacade;
            _commandSender = commandSender;
        }

        public async Task<IViewComponentResult> InvokeAsync(ModuleModel model)
        {
            var content = _textModuleFacade.GetContent(model.Id, _contextService.GetCurrentLanguageInfo().Id);

            if (content == null)
            {
                var defaultContent = "Your content here...";

                await Task.Run(() => _commandSender.Send<CreateTextModule, TextModule>(new CreateTextModule
                {
                    SiteId = SiteId,
                    Id = Guid.NewGuid(),
                    ModuleId = model.Id,
                    Content = defaultContent
                }));

                content = defaultContent;
            }

            return View("~/Apps/Weapsy.Apps.Text/Views/Shared/Components/TextModule/Default.cshtml", content);
        }
    }
}
