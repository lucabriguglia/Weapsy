using System.Collections.Generic;

namespace Weapsy.Mvc.Apps
{
    public interface IAppLoader
    {
        IList<AppDescriptor> LoadApps();
    }
}
