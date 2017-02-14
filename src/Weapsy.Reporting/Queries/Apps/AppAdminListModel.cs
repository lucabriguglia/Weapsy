using System;
using Weapsy.Domain.Apps;

namespace Weapsy.Reporting.Apps
{
    public class AppAdminListModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public AppStatus Status { get; set; }
    }
}
