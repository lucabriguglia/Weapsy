using System;
using System.Collections.Generic;
using Weapsy.Domain.Languages;
using Weapsy.Domain.Pages;

namespace Weapsy.Reporting.Pages
{
    public class PageAdminModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeywords { get; set; }
        public PageStatus Status { get; set; }
        public List<PageLocalisationAdminModel> PageLocalisations { get; set; } = new List<PageLocalisationAdminModel>();
        public List<PagePermissionModel> PagePermissions { get; set; } = new List<PagePermissionModel>();
        public List<MenuModel> Menus { get; set; } = new List<MenuModel>();
    }

    public class PageLocalisationAdminModel
    {
        public Guid PageId { get; set; }
        public Guid LanguageId { get; set; }
        public string LanguageName { get; set; }
        public LanguageStatus LanguageStatus { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeywords { get; set; }
    }

    public class PagePermissionModel
    {
        public Guid PageId { get; set; }
        public Guid RoleId { get; set; }
        public string RoleName { get; set; }
        public bool Disabled { get; set; }
        public List<PagePermissionTypeModel> PagePermissionTypes { get; set; } = new List<PagePermissionTypeModel>();
    }

    public class PagePermissionTypeModel
    {
        public PermissionType Type { get; set; }
        public bool Selected { get; set; }
    }

    public class MenuModel
    {
        public Guid MenuId { get; set; }
        public string MenuName { get; set; }
        public bool Selected { get; set; }
    }
}
