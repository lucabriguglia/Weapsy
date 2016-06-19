using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Weapsy.Reporting.Modules
{
    public interface IModuleFacade
    {
        Task<IEnumerable<ModuleAdminListModel>> GetAllForAdminAsync();
        Task<ModuleAdminModel> GetAdminModelAsync(Guid id);        
    }
}
