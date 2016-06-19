using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Weapsy.Reporting.Themes
{
    public interface IThemeFacade
    {
        Task<IEnumerable<ThemeAdminModel>> GetAllForAdminAsync();
        Task<ThemeAdminModel> GetForAdminAsync(Guid id);
    }
}
