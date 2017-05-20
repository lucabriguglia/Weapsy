using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Weapsy.Apps.Text.Domain;
using Weapsy.Apps.Text.Domain.Commands;
using Weapsy.Apps.Text.Reporting;
using Weapsy.Framework.Queries;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Weapsy.Reporting.Languages;
using Weapsy.Reporting.Languages.Queries;

namespace Weapsy.Apps.Text.Data.Reporting
{
    public class GetAdminModelHandler : IQueryHandlerAsync<GetAdminModel, AddVersion>
    {
        private readonly IContextFactory _contextFactory;
        private readonly IQueryDispatcher _queryDispatcher;

        public GetAdminModelHandler(IContextFactory contextFactory, IQueryDispatcher queryDispatcher)
        {
            _contextFactory = contextFactory;
            _queryDispatcher = queryDispatcher;
        }

        public async Task<AddVersion> RetrieveAsync(GetAdminModel query)
        {
            using (var context = _contextFactory.Create())
            {
                var textModule = await context.TextModules.FirstOrDefaultAsync(x => x.ModuleId == query.ModuleId && x.Status == TextModuleStatus.Active);

                if (textModule == null)
                    return new AddVersion();

                textModule.TextVersions = await context.TextVersions
                    .Include(y => y.TextLocalisations)
                    .Where(x => x.TextModuleId == textModule.Id && x.Status != TextVersionStatus.Deleted)
                    .ToListAsync();

                var version = query.VersionId != Guid.Empty
                    ? textModule.TextVersions.FirstOrDefault(x => x.Id == query.VersionId)
                    : textModule.TextVersions.FirstOrDefault(x => x.Status == TextVersionStatus.Published);

                if (version == null)
                    return new AddVersion();

                var result = new AddVersion
                {
                    Id = textModule.Id,
                    ModuleId = textModule.ModuleId,
                    Content = version.Content,
                    Description = version.Description
                };

                var languages = await _queryDispatcher.DispatchAsync<GetAllActive, IEnumerable<LanguageInfo>>(new GetAllActive { SiteId = query.SiteId });

                foreach (var language in languages)
                {
                    var content = string.Empty;

                    var existingLocalisation = version
                        .TextLocalisations
                        .FirstOrDefault(x => x.LanguageId == language.Id);

                    if (existingLocalisation != null)
                    {
                        content = existingLocalisation.Content;
                    }

                    result.VersionLocalisations.Add(new AddVersion.VersionLocalisation
                    {
                        LanguageId = language.Id,
                        LanguageName = language.Name,
                        Content = content
                    });
                }

                return result;
            }
        }
    }
}
