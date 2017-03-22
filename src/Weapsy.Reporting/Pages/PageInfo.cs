using System;
using System.Collections.Generic;
using System.Linq;
using Weapsy.Domain.ModuleTypes;
using Weapsy.Domain.Pages;

namespace Weapsy.Reporting.Pages
{
    public class PageInfo
    {
        public PageModel Page { get; set; }
        public ICollection<ZoneModel> Zones { get; set; } = new List<ZoneModel>();

        public ZoneModel Zone(string zoneName)
        {
            var zone = Zones.FirstOrDefault(x => x.Name.ToLowerInvariant() == zoneName.ToLowerInvariant());
            return zone ?? new ZoneModel();
        }
    }

    public class PageModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeywords { get; set; }
        public string Theme { get; set; }
        public string Template { get; set; }
        public Dictionary<PermissionType, IEnumerable<string>> Roles { get; set; }
    }

    public class ZoneModel
    {
        public string Name { get; set; }
        public ICollection<ModuleModel> Modules { get; set; } = new List<ModuleModel>();
    }

    public class ModuleModel
    {
        public Guid Id { get; set; }
        public Guid PageModuleId { get; set; }
        public string Title { get; set; }
        public string Zone { get; set; }
        public int SortOrder { get; set; }
        public string Template { get; set; }
        public ModuleTypeModel ModuleType { get; set; } = new ModuleTypeModel();
        public Dictionary<PermissionType, IEnumerable<string>> Roles { get; set; }
    }

    public class ModuleTypeModel
    {
        public ViewType ViewType { get; set; }
        public string ViewName { get; set; }
        public EditType EditType { get; set; }
        public string EditUrl { get; set; }
    }
}
