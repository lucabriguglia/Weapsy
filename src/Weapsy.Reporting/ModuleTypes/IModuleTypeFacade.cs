using System;
using System.Collections.Generic;

namespace Weapsy.Reporting.ModuleTypes
{
    public interface IModuleTypeFacade
    {
        IEnumerable<ModuleTypeAdminListModel> GetAllForAdmin(Guid appId = new Guid());
        ModuleTypeAdminModel GetAdminModel(Guid moduleTypeId);
        ModuleTypeAdminModel GetDefaultAdminModel();
        IEnumerable<ModuleTypeControlPanelModel> GetControlPanelModel();
    }
}
