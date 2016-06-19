using System;
using System.Collections.Generic;
using FluentValidation;
using Weapsy.Core.Domain;
using Weapsy.Domain.Model.Menus.Commands;
using Weapsy.Domain.Model.Menus.Events;
using System.Linq;

namespace Weapsy.Domain.Model.Menus
{
    public class Menu : AggregateRoot
    {
        public Guid SiteId { get; private set; }
        public string Name { get; private set; }
        public MenuStatus Status { get; private set; }
        public IList<MenuItem> MenuItems { get; private set; } = new List<MenuItem>();

        public Menu(){}

        private Menu(CreateMenu cmd) : base(cmd.Id)
        {
            SiteId = cmd.SiteId;
            Name = cmd.Name;
            Status = MenuStatus.Active;

            AddEvent(new MenuCreated
            {
                SiteId = SiteId,
                AggregateRootId = Id,
                Name = Name,
                Status = Status
            });
        }

        public static Menu CreateNew(CreateMenu cmd, IValidator<CreateMenu> validator)
        {
            validator.ValidateCommand(cmd);

            return new Menu(cmd);
        }

        public void UpdateName()
        {
            throw new NotImplementedException();
        }

        public void AddMenuItem(AddMenuItem cmd, IValidator<AddMenuItem> validator)
        {
            if (MenuItems.FirstOrDefault(x => x.Id == cmd.MenuItemId) != null)
                throw new Exception("Menu item already added");

            validator.ValidateCommand(cmd);

            var sortOrder = MenuItems.Where(x => x.ParentId == Guid.Empty).Count() + 1;

            MenuItems.Add(new MenuItem(cmd, sortOrder));

            AddEvent(new MenuItemAdded
            {
                SiteId = SiteId,
                AggregateRootId = Id,
                MenuItem = MenuItems.Single(x => x.Id == cmd.MenuItemId)
            });
        }

        public void UpdateMenuItem(UpdateMenuItem cmd, IValidator<UpdateMenuItem> validator)
        {
            var menuItem = MenuItems.FirstOrDefault(x => x.Id == cmd.MenuItemId);

            if (menuItem == null || menuItem.Status == MenuItemStatus.Deleted)
                throw new Exception("Menu item not found");

            validator.ValidateCommand(cmd);

            menuItem.Update(cmd);

            AddEvent(new MenuItemUpdated
            {
                SiteId = SiteId,
                AggregateRootId = Id,
                MenuItem = menuItem
            });
        }

        public void ReorderMenuItems(ReorderMenuItems cmd, IValidator<ReorderMenuItems> validator)
        {
            validator.ValidateCommand(cmd);

            var reorderedMenuItems = new List<MenuItemsReordered.MenuItem>();

            var groupsByParent = cmd.MenuItems.GroupBy(x => x.ParentId).ToList();

            foreach (var group in groupsByParent)
            {
                var parentId = group.Key;

                if (parentId != Guid.Empty && MenuItems.FirstOrDefault(x => x.Id == parentId) == null)
                    throw new Exception("Parent menu item not found.");

                var items = group.ToList();

                for (int i = 0; i < items.Count; i++)
                {
                    var id = items[i].Id;
                    var sortOrder = i + 1;

                    var menuItem = MenuItems.FirstOrDefault(x => x.Id == id);

                    if (menuItem == null || menuItem.Status == MenuItemStatus.Deleted)
                        throw new Exception("Menu item not found.");

                    if (menuItem.ParentId == parentId && menuItem.SortOrder == sortOrder)
                        continue;

                    menuItem.Reorder(parentId, sortOrder);

                    reorderedMenuItems.Add(new MenuItemsReordered.MenuItem
                    {
                        Id = id,
                        ParentId = parentId,
                        SortOrder = sortOrder
                    });
                }
            }

            AddEvent(new MenuItemsReordered
            {
                SiteId = SiteId,
                AggregateRootId = Id,
                Name = Name,
                MenuItems = reorderedMenuItems
            });
        }

        public void RemoveMenuItem(RemoveMenuItem cmd, IValidator<RemoveMenuItem> validator)
        {
            validator.ValidateCommand(cmd);

            var menuItemToRemove = MenuItems.FirstOrDefault(x => x.Id == cmd.MenuItemId);

            if (menuItemToRemove == null || menuItemToRemove.Status == MenuItemStatus.Deleted)
                throw new Exception("Menu item to remove not found.");

            MarkMenuItemAsDeleted(menuItemToRemove);

            AddEvent(new MenuItemRemoved
            {
                SiteId = SiteId,
                AggregateRootId = Id,
                MenuItemId = menuItemToRemove.Id
            });
        }

        private void MarkMenuItemAsDeleted(MenuItem menuItem)
        {
            menuItem.Delete();

            var subMenuItemsToDelete = MenuItems
                .Where(x => x.ParentId == menuItem.Id && x.Status == MenuItemStatus.Active)
                .ToList();

            foreach (var subMenuItem in subMenuItemsToDelete)
            {
                MarkMenuItemAsDeleted(subMenuItem);
            }
        }

        public void Delete()
        {
            if (Status == MenuStatus.Deleted)
                throw new Exception("Menu already deleted.");

            if (Name.ToLowerInvariant() == "main")
                throw new Exception("Main menu cannot be deleted.");

            Status = MenuStatus.Deleted;

            AddEvent(new MenuDeleted
            {
                SiteId = SiteId,
                AggregateRootId = Id,
                Name = Name
            });
        }

        public void Restore()
        {
            throw new NotImplementedException();
        }
    }
}