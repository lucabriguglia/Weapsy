using System;
using System.Collections.Generic;
using Weapsy.Reporting.Modules;

namespace Weapsy.Reporting.Data.Modules
{
    public class ModuleFacade : IModuleFacade
    {
        public ModuleAdminModel GetAdminModel(Guid id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ModuleAdminListModel> GetAllForAdmin()
        {
            throw new NotImplementedException();
        }
    }
}
