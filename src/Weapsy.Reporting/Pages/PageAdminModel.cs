using System;
using System.Collections.Generic;
using Weapsy.Domain.Model.Pages;

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
    }

    public class PageLocalisationAdminModel
    {
        public Guid PageId { get; set; }
        public Guid LanguageId { get; set; }
        public string LanguageName { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeywords { get; set; }
    }

    public class PagePermissionModel
    {
        public Guid PageId { get; set; }
        public PermissionType Type { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public bool Selected { get; set; }
    }
}
