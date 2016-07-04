using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Weapsy.Domain.Model.Apps;
using Weapsy.Reporting.Apps;

namespace Weapsy.Reporting.Data.Default.Apps
{
    public class AppFacade : IAppFacade
    {
        private readonly IAppRepository _appRepository;
        private readonly IMapper _mapper;

        public AppFacade(IAppRepository appRepository, IMapper mapper)
        {
            _appRepository = appRepository;
            _mapper = mapper;
        }

        public IEnumerable<AppAdminListModel> GetAllForAdmin()
        {
            var apps = _appRepository.GetAll().Where(x => x.Status != AppStatus.Deleted);
            return _mapper.Map<IEnumerable<AppAdminListModel>>(apps);
        }

        public AppAdminModel GetAdminModel(Guid appId)
        {
            var app = _appRepository.GetById(appId);

            if (app == null || app.Status == AppStatus.Deleted)
                return null;

            return _mapper.Map<AppAdminModel>(app);
        }

        public AppAdminModel GetDefaultAdminModel()
        {
            return new AppAdminModel();
        }
    }
}