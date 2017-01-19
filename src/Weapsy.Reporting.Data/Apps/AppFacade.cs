using System;
using System.Collections.Generic;
using AutoMapper;
using Weapsy.Data;
using Weapsy.Domain.Apps;
using Weapsy.Reporting.Apps;
using System.Linq;

namespace Weapsy.Reporting.Data.Apps
{
    public class AppFacade : IAppFacade
    {
        private readonly IDbContextFactory _dbContextFactory;
        private readonly IMapper _mapper;

        public AppFacade(IDbContextFactory dbContextFactory, IMapper mapper)
        {
            _dbContextFactory = dbContextFactory;
            _mapper = mapper;
        }

        public IEnumerable<AppAdminListModel> GetAllForAdmin()
        {
            using (var context = _dbContextFactory.Create())
            {
                var dbEntities = context.Apps
                    .Where(x => x.Status != AppStatus.Deleted)
                    .ToList();

                return _mapper.Map<IEnumerable<AppAdminListModel>>(dbEntities);
            }
        }

        public AppAdminModel GetForAdmin(Guid appId)
        {
            using (var context = _dbContextFactory.Create())
            {
                var dbEntity = context.Apps.FirstOrDefault(x => x.Id == appId && x.Status != AppStatus.Deleted);
                return dbEntity != null ? _mapper.Map<AppAdminModel>(dbEntity) : null;
            }
        }

        public AppAdminModel GetDefaultForAdmin()
        {
            return new AppAdminModel();
        }
    }
}