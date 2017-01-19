using System;
using System.Collections.Generic;
using AutoMapper;
using Weapsy.Data;
using Weapsy.Domain.Apps;
using Weapsy.Domain.ModuleTypes;
using Weapsy.Infrastructure.Caching;
using Weapsy.Reporting.ModuleTypes;
using System.Linq;

namespace Weapsy.Reporting.Data.ModuleTypes
{
    public class ModuleTypeFacade : IModuleTypeFacade
    {
        private readonly IDbContextFactory _dbContextFactory;
        private readonly ICacheManager _cacheManager;
        private readonly IMapper _mapper;

        public ModuleTypeFacade(IDbContextFactory dbContextFactory,
            ICacheManager cacheManager,
            IMapper mapper)
        {
            _dbContextFactory = dbContextFactory;
            _cacheManager = cacheManager;
            _mapper = mapper;
        }

        public IEnumerable<ModuleTypeAdminListModel> GetAllForAdmin(Guid appId = default(Guid))
        {
            using (var context = _dbContextFactory.Create())
            {
                var dbEntities = context.ModuleTypes
                    .Where(x => x.Status != ModuleTypeStatus.Deleted)
                    .ToList();

                return _mapper.Map<IEnumerable<ModuleTypeAdminListModel>>(dbEntities);
            }
        }

        public ModuleTypeAdminModel GetAdminModel(Guid moduleTypeId)
        {
            using (var context = _dbContextFactory.Create())
            {
                var dbEntity = context.ModuleTypes
                    .FirstOrDefault(x => x.Id == moduleTypeId && x.Status != ModuleTypeStatus.Deleted);

                if (dbEntity == null)
                    return null;

                var result = _mapper.Map<ModuleTypeAdminModel>(dbEntity);

                var apps = context.Apps
                    .Where(x => x.Status != AppStatus.Deleted)
                    .Select(app => new ModuleTypeAdminModel.App
                    {
                        Id = app.Id,
                        Name = app.Name
                    }).ToList();

                result.AvailableApps.AddRange(apps);

                return result;
            }
        }

        public ModuleTypeAdminModel GetDefaultAdminModel()
        {
            using (var context = _dbContextFactory.Create())
            {
                var result = new ModuleTypeAdminModel();

                var apps = context.Apps
                    .Where(x => x.Status != AppStatus.Deleted)
                    .Select(app => new ModuleTypeAdminModel.App
                    {
                        Id = app.Id,
                        Name = app.Name
                    }).ToList();

                result.AvailableApps.AddRange(apps);

                return result;
            }
        }

        public IEnumerable<ModuleTypeControlPanelModel> GetControlPanelModel()
        {
            return _cacheManager.Get(CacheKeys.ModuleTypesCacheKey, () =>
            {
                using (var context = _dbContextFactory.Create())
                {
                    return context.ModuleTypes
                        .Where(x => x.Status != ModuleTypeStatus.Deleted)
                        .Select(moduleType => new ModuleTypeControlPanelModel
                        {
                            Id = moduleType.Id,
                            Title = moduleType.Title
                        })
                        .ToList();
                }
            });
        }
    }
}