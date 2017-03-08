using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Weapsy.Apps.Text.Data;
using Weapsy.Domain.Apps.Commands;
using Weapsy.Domain.ModuleTypes;
using Weapsy.Domain.ModuleTypes.Commands;
using Weapsy.Services.Installation;

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
            var appInstallationService = builder.ApplicationServices.GetRequiredService<IAppInstallationService>();

            var createApp = new CreateApp
            {
                Id = Guid.NewGuid(),
                Name = "Weapsy.Apps.Text",
                Description = "Text App",
                Folder = "Weapsy.Apps.Text"
            };

            var createModuleTypes = new List<CreateModuleType>
            {
                new CreateModuleType
                {
                    AppId = createApp.Id,
                    Id = Guid.NewGuid(),
                    Name = "Text",
                    Title = "Text Module",
                    Description = "Text Module",
                    ViewType = ViewType.ViewComponent,
                    ViewName = "TextModule",
                    EditType = EditType.Modal,
                    EditUrl = "Home/Index"
                }
            };

            appInstallationService.EnsureAppInstalled(createApp, createModuleTypes);

            return builder;
        }
    }
}
