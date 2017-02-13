using System;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Weapsy.Apps.Text.Data;
using Weapsy.Domain.Apps;
using Weapsy.Domain.Apps.Commands;
using Weapsy.Domain.ModuleTypes;
using Weapsy.Domain.ModuleTypes.Commands;

namespace Weapsy.Apps.Text.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder EnsureDbCreated(this IApplicationBuilder builder)
        {
            var dbContext = builder.ApplicationServices.GetRequiredService<TextDbContext>();

            dbContext.Database.Migrate();

            return builder;
        }

        public static IApplicationBuilder EnsureAppInstalled(this IApplicationBuilder builder)
        {
            var appRepository = builder.ApplicationServices.GetRequiredService<IAppRepository>();

            if (appRepository.GetByName("Weapsy.Apps.Text") != null)
                return builder;

            var createAppValidator = builder.ApplicationServices.GetRequiredService<IValidator<CreateApp>>();
            var createModuleTypeValidator = builder.ApplicationServices.GetRequiredService<IValidator<CreateModuleType>>();            
            var moduleTypeRepository = builder.ApplicationServices.GetRequiredService<IModuleTypeRepository>();

            var appId = Guid.NewGuid();

            var textApp = App.CreateNew(new CreateApp
            {
                Id = appId,
                Name = "Weapsy.Apps.Text",
                Description = "Text App",
                Folder = "Weapsy.Apps.Text"
            }, createAppValidator);

            appRepository.Create(textApp);

            var moduleType = ModuleType.CreateNew(new CreateModuleType
            {
                AppId = appId,
                Id = Guid.NewGuid(),
                Name = "Text",
                Title = "Text Module",
                Description = "Text Module",
                ViewType = ViewType.ViewComponent,
                ViewName = "TextModule",
                EditType = EditType.Modal,
                EditUrl = "Weapsy.Apps.Text/Home/Index"
            }, createModuleTypeValidator);

            moduleTypeRepository.Create(moduleType);

            return builder;
        }
    }
}
