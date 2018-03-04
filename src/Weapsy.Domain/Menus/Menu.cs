using System;
using System.Collections.Generic;
using FluentValidation;
using Weapsy.Framework.Domain;
using Weapsy.Domain.Menus.Commands;
using Weapsy.Domain.Menus.Events;
using System.Linq;
using Weapsy.Cqrs.Domain;

namespace Weapsy.Domain.Menus
{
    public class Menu : AggregateRoot
    {
        public Guid SiteId { get; private set; }
        public string Name { get; private set; }
        public MenuStatus Status { get; private set; }
        public ICollection<MenuItem> MenuItems { get; private set; } = new List<MenuItem>();
        
        public Menu()
        {
        }

        private Menu(CreateMenu cmd) : base(cmd.Id)
        {
            AddEvent(new MenuCreated
            {
                SiteId = cmd.SiteId,
                AggregateRootId = Id,
                Name = cmd.Name,
                Status = MenuStatus.Active
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

            var sortOrder = MenuItems.Count(x => x.ParentId == Guid.Empty) + 1;
            var newMenuItem = new MenuItem(cmd, sortOrder);;

            AddEvent(new MenuItemAdded
            {
                SiteId = SiteId,
                AggregateRootId = Id,
                Name = Name,
                MenuItem = newMenuItem
            });
        }

        public void UpdateMenuItem(UpdateMenuItem cmd, IValidator<UpdateMenuItem> validator)
        {
            var menuItem = MenuItems.FirstOrDefault(x => x.Id == cmd.MenuItemId);

            if (menuItem == null || menuItem.Status == MenuItemStatus.Deleted)
                throw new Exception("Menu item not found");

            validator.ValidateCommand(cmd);

            AddEvent(new MenuItemUpdated
            {
                SiteId = SiteId,
                AggregateRootId = Id,
                MenuName = Name,
                MenuItem = new MenuItem(cmd, menuItem.SortOrder)
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
                    throw new ApplicationException("Parent menu item not found.");

                var items = group.ToList();

                for (int i = 0; i < items.Count; i++)
                {
                    var id = items[i].Id;
                    var sortOrder = i + 1;

                    var menuItem = MenuItems.FirstOrDefault(x => x.Id == id);

                    if (menuItem == null || menuItem.Status == MenuItemStatus.Deleted)
                        throw new ApplicationException("Menu item not found.");

                    if (menuItem.ParentId == parentId && menuItem.SortOrder == sortOrder)
                        continue;

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

            AddEvent(new MenuItemRemoved
            {
                SiteId = SiteId,
                AggregateRootId = Id,
                Name = Name,
                MenuItemId = menuItemToRemove.Id
            });
        }

        public void SetMenuItemPermissions(SetMenuItemPermissions cmd)
        {
            var menuItem = MenuItems.FirstOrDefault(x => x.Id == cmd.MenuItemId);

            if (menuItem == null)
                throw new Exception("Menu item not found.");

            Events.Add(new MenuItemPermissionsSet
            {
                SiteId = SiteId,
                AggregateRootId = Id,
                MenuItemId = cmd.MenuItemId,
                MenuItemPermissions = cmd.MenuItemPermissions
            });
        }

        public void Delete()
        {
            if (Status == MenuStatus.Deleted)
                throw new Exception("Menu already deleted.");

            if (Name.ToLowerInvariant() == "main")
                throw new Exception("Main menu cannot be deleted.");

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

        private void Apply(MenuCreated @event)
        {
            Id = @event.AggregateRootId;
            SiteId = @event.SiteId;
            Name = @event.Name;
            Status = @event.Status;
        }

        private void Apply(MenuItemAdded @event)
        {
            MenuItems.Add(@event.MenuItem);
        }

        private void Apply(MenuItemUpdated @event)
        {
            var menuItem = MenuItems.FirstOrDefault(x => x.Id == @event.MenuItem.Id);
            menuItem?.Update(@event.MenuItem);
        }

        private void Apply(MenuItemsReordered @event)
        {
            foreach (var item in @event.MenuItems)
            {
                var menuItem = MenuItems.FirstOrDefault(x => x.Id == item.Id);
                menuItem?.Reorder(item.ParentId, item.SortOrder);
            }
        }

        private void Apply(MenuItemRemoved @event)
        {
            var menuItem = MenuItems.FirstOrDefault(x => x.Id == @event.MenuItemId);
            if (menuItem != null)
                MarkMenuItemAsDeleted(menuItem);
        }

        private void MarkMenuItemAsDeleted(MenuItem menuItem)
        {
            menuItem.Delete();

            var subMenuItemsToDelete = MenuItems
                .Where(x => x.ParentId == menuItem.Id && x.Status == MenuItemStatus.Active)
                .ToList();

            foreach (var subMenuItem in subMenuItemsToDelete)
                MarkMenuItemAsDeleted(subMenuItem);
        }

        private void Apply(MenuItemPermissionsSet @event)
        {
            var menuItem = MenuItems.FirstOrDefault(x => x.Id == @event.MenuItemId);
            menuItem?.SetPermisisons(@event.MenuItemPermissions);

        }

        private void Apply(MenuDeleted @event)
        {
            Status = MenuStatus.Deleted;
        }
    }
}