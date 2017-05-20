using System;
using System.Collections.Generic;
using System.Linq;
using Weapsy.Framework.Domain;
using Weapsy.Domain.Menus.Commands;
using Weapsy.Framework.Identity;

namespace Weapsy.Domain.Menus
{
    public class MenuItem : Entity
    {
        public Guid MenuId { get; private set; }
        public Guid ParentId { get; private set; }
        public int SortOrder { get; private set; }
        public MenuItemType Type { get; private set; }
        public Guid PageId { get; private set; }
        public string Link { get; private set; }
        public string Text { get; private set; }
        public string Title { get; private set; }
        public MenuItemStatus Status { get; private set; }
        public ICollection<MenuItemLocalisation> MenuItemLocalisations { get; private set; } = new List<MenuItemLocalisation>();
        public ICollection<MenuItemPermission> MenuItemPermissions { get; private set; } = new List<MenuItemPermission>();

        public MenuItem() {}

        public MenuItem(AddMenuItem cmd, int sortOrder) 
            : base(cmd.MenuItemId)
        {
            MenuId = cmd.MenuId;
            SortOrder = sortOrder;
            Type = cmd.Type;
            PageId = cmd.PageId;
            Link = cmd.Link;
            Text = cmd.Text;
            Title = cmd.Title;
            Status = MenuItemStatus.Active;

            SetLocalisations(cmd.MenuItemLocalisations);
            SetPermisisons(cmd.MenuItemPermissions);
        }

        public void Update(UpdateMenuItem cmd)
        {
            Type = cmd.Type;
            PageId = cmd.PageId;
            Link = cmd.Link;
            Text = cmd.Text;
            Title = cmd.Title;

            SetLocalisations(cmd.MenuItemLocalisations);
            SetPermisisons(cmd.MenuItemPermissions);
        }

        private void SetLocalisations(IEnumerable<MenuItemLocalisation> localisations)
        {
            MenuItemLocalisations.Clear();

            foreach (var item in localisations)
            {
                if (MenuItemLocalisations.FirstOrDefault(x => x.LanguageId == item.LanguageId) != null)
                    continue;

                MenuItemLocalisations.Add(new MenuItemLocalisation(Id, item.LanguageId, item.Text, item.Title));
            }
        }

        public void SetPermisisons(IEnumerable<MenuItemPermission> permissions)
        {
            MenuItemPermissions.Clear();

            foreach (var permission in permissions)
            {
                if (MenuItemPermissions.FirstOrDefault(x => x.RoleId == permission.RoleId) != null)
                    continue;

                MenuItemPermissions.Add(new MenuItemPermission
                {
                    MenuItemId = Id,
                    RoleId = permission.RoleId
                });
            }

            if (!MenuItemPermissions.Any())
                MenuItemPermissions.Add(new MenuItemPermission
                {
                    MenuItemId = Id,
                    RoleId = Everyone.Id
                });
        }

        public void Reorder(Guid parentId, int sortOrder)
        {
            ParentId = parentId;
            SortOrder = sortOrder;
        }

        public void Delete()
        {
            if (Status == MenuItemStatus.Deleted)
                throw new Exception("Menu item already deleted.");

            Status = MenuItemStatus.Deleted;
        }
    }
}