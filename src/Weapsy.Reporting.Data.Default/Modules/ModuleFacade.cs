using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Weapsy.Reporting.Modules;

namespace Weapsy.Reporting.Data.Modules
{
    public class ModuleFacade : IModuleFacade
    {
        public Task<ModuleAdminModel> GetAdminModelAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ModuleAdminListModel>> GetAllForAdminAsync()
        {
            throw new NotImplementedException();
        }
    }
}
