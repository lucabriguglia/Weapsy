using System;
using System.Collections.Generic;

namespace Weapsy.Reporting.Modules
{
    public interface IModuleFacade
    {
        IEnumerable<ModuleAdminListModel> GetAllForAdmin();
        ModuleAdminModel GetAdminModel(Guid id);        
    }
}
