using System;
using System.Collections.Generic;

namespace Weapsy.Reporting.Apps
{
    public interface IAppFacade
    {
        IEnumerable<AppAdminListModel> GetAllForAdmin();
        AppAdminModel GetAdminModel(Guid appId);
        AppAdminModel GetDefaultAdminModel();
    }
}
