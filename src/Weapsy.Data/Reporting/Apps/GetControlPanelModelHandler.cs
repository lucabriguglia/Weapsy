using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Weapsy.Domain.ModuleTypes;
using Weapsy.Framework.Caching;
using Weapsy.Framework.Queries;
using Weapsy.Reporting.Apps;
using Weapsy.Reporting.Apps.Queries;

namespace Weapsy.Data.Reporting.Apps
{
    public class GetControlPanelModelHandler : IQueryHandlerAsync<GetControlPanelModel, IEnumerable<ModuleTypeControlPanelModel>>
    {
        private readonly IContextFactory _contextFactory;
        private readonly IMapper _mapper;
        private readonly ICacheManager _cacheManager;

        public GetControlPanelModelHandler(IContextFactory contextFactory, IMapper mapper, ICacheManager cacheManager)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
            _cacheManager = cacheManager;
        }

        public async Task<IEnumerable<ModuleTypeControlPanelModel>> RetrieveAsync(GetControlPanelModel query)
        {
            return await _cacheManager.Get(CacheKeys.ModuleTypesCacheKey, async () =>
            {
                using (var context = _contextFactory.Create())
                {
                    return await context.ModuleTypes
                        .Where(x => x.Status != ModuleTypeStatus.Deleted)
                        .Select(moduleType => new ModuleTypeControlPanelModel
                        {
                            Id = moduleType.Id,
                            Title = moduleType.Title
                        })
                        .ToListAsync();
                }
            });
        }
    }
}
