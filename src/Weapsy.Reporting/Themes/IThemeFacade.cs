using System;
using System.Collections.Generic;

namespace Weapsy.Reporting.Themes
{
    public interface IThemeFacade
    {
        IEnumerable<ThemeAdminModel> GetAllForAdmin();
        ThemeAdminModel GetForAdmin(Guid id);
    }
}
