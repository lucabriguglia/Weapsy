using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Weapsy.Core.Caching;
using Weapsy.Domain.Apps;
using Weapsy.Domain.ModuleTypes;
using Weapsy.Reporting.ModuleTypes;

namespace Weapsy.Reporting.Data.Default.ModuleTypes
{
    public class ModuleTypeFacade : IModuleTypeFacade
    {
        private readonly IModuleTypeRepository _moduleTypeRepository;
        private readonly IAppRepository _appRepository;
        private readonly ICacheManager _cacheManager;
        private readonly IMapper _mapper;

        public ModuleTypeFacade(IModuleTypeRepository moduleTypeRepository,
            IAppRepository appRepository,
            ICacheManager cacheManager,
            IMapper mapper)
        {
            _moduleTypeRepository = moduleTypeRepository;
            _appRepository = appRepository;
            _cacheManager = cacheManager;
            _mapper = mapper;
        }

        public IEnumerable<ModuleTypeAdminListModel> GetAllForAdmin(Guid appId = default(Guid))
        {
            var moduleTypes = _moduleTypeRepository.GetAll().Where(x => x.Status != ModuleTypeStatus.Deleted);
            return _mapper.Map<IEnumerable<ModuleTypeAdminListModel>>(moduleTypes);
        }

        public ModuleTypeAdminModel GetAdminModel(Guid moduleTypeId)
        {
            var moduleType = _moduleTypeRepository.GetById(moduleTypeId);
            if (moduleType == null || moduleType.Status == ModuleTypeStatus.Deleted)
                return null;
            var result = _mapper.Map<ModuleTypeAdminModel>(moduleType);
            foreach (var app in _appRepository.GetAll())
                result.AvailableApps.Add(new ModuleTypeAdminModel.App { Id = app.Id, Name = app.Name });
            return result;
        }

        public ModuleTypeAdminModel GetDefaultAdminModel()
        {
            var result = new ModuleTypeAdminModel();
            foreach (var app in _appRepository.GetAll())
                result.AvailableApps.Add(new ModuleTypeAdminModel.App { Id = app.Id, Name = app.Name });
            return result;
        }

        public IEnumerable<ModuleTypeControlPanelModel> GetControlPanelModel()
        {
            return _cacheManager.Get(CacheKeys.ModuleTypesCacheKey, () =>
            {
                var moduleTypes = _moduleTypeRepository.GetAll();
                var result = new List<ModuleTypeControlPanelModel>();
                foreach (var moduleType in moduleTypes)
                {
                    result.Add(new ModuleTypeControlPanelModel
                    {
                        Id = moduleType.Id,
                        Title = moduleType.Title
                    });
                }
                return result;
            });
        }
    }
}