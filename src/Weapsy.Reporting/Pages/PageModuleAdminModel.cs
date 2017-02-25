using System;
using System.Collections.Generic;
using Weapsy.Domain.Languages;
using Weapsy.Domain.Pages;

namespace Weapsy.Reporting.Pages
{
    public class PageModuleAdminModel
    {
        public Guid PageId { get; set; }
        public Guid ModuleId { get; set; }
        public Guid PageModuleId { get; set; }
        public string Title { get; set; }
        public bool InheritPermissions { get; set; }
        public List<PageModuleLocalisationAdminModel> PageModuleLocalisations { get; set; } = new List<PageModuleLocalisationAdminModel>();
        public List<PageModulePermissionModel> PageModulePermissions { get; set; } = new List<PageModulePermissionModel>();
    }

    public class PageModuleLocalisationAdminModel
    {
        public Guid PageModuleId { get; set; }
        public Guid LanguageId { get; set; }
        public string LanguageName { get; set; }
        public LanguageStatus LanguageStatus { get; set; }
        public string Title { get; set; }
    }

    public class PageModulePermissionModel
    {
        public Guid PageModuleId { get; set; }
        public Guid RoleId { get; set; }
        public string RoleName { get; set; }
        public bool Disabled { get; set; }
        public List<PageModulePermissionTypeModel> PageModulePermissionTypes { get; set; } = new List<PageModulePermissionTypeModel>();
    }

    public class PageModulePermissionTypeModel
    {
        public PermissionType Type { get; set; }
        public bool Selected { get; set; }
    }
}
