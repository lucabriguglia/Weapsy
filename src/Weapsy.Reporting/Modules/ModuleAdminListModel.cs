using System;
using Weapsy.Domain.Modules;

namespace Weapsy.Reporting.Modules
{
    public class ModuleAdminListModel
    {
        public Guid Id { get; set; }
        public Guid ModuleTypeId { get; set; }
        public string ModuleTypeName { get; set; }
        public string Title { get; set; }
        public ModuleStatus Status { get; set; }
    }
}
