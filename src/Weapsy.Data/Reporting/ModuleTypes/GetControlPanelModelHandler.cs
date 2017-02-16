using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Weapsy.Domain.ModuleTypes;
using Weapsy.Infrastructure.Queries;
using Weapsy.Reporting.ModuleTypes;
using Weapsy.Reporting.ModuleTypes.Queries;
using System.Linq;
using Weapsy.Domain.Apps;
using Weapsy.Infrastructure.Caching;
using Microsoft.EntityFrameworkCore;

namespace Weapsy.Data.Reporting.ModuleTypes
{
    public class GetControlPanelModelHandler : IQueryHandlerAsync<GetControlPanelModel, IEnumerable<ModuleTypeControlPanelModel>>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;
        private readonly ICacheManager _cacheManager;

        public GetControlPanelModelHandler(IDbContextFactory contextFactory, IMapper mapper, ICacheManager cacheManager)
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
