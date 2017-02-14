using System;
using System.Collections.Generic;
using Weapsy.Domain.Languages;
using Weapsy.Domain.Menus;

namespace Weapsy.Reporting.Menus
{
    public class MenuItemAdminModel
    {
        public Guid Id { get; set; }
        public MenuItemType Type { get; set; }
        public Guid PageId { get; set; }
        public string Link { get; set; }
        public string Text { get; set; }
        public string Title { get; set; }
        public List<MenuItemLocalisation> MenuItemLocalisations { get; set; } = new List<MenuItemLocalisation>();
        public List<MenuItemPermission> MenuItemPermissions { get; set; } = new List<MenuItemPermission>();

        public class MenuItemLocalisation
        {
            public Guid MenuItemId { get; set; }
            public Guid LanguageId { get; set; }            
            public string LanguageName { get; set; }
            public LanguageStatus LanguageStatus { get; set; }
            public string Text { get; set; }
            public string Title { get; set; }
        }

        public class MenuItemPermission
        {
            public Guid MenuItemId { get; set; }
            public string RoleId { get; set; }
            public string RoleName { get; set; }
            public bool Selected { get; set; }
            public bool Disabled { get; set; }
        }
    }
}
