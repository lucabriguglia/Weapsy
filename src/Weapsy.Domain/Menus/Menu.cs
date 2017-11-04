using System;
using System.Collections.Generic;
using FluentValidation;
using Weapsy.Framework.Domain;
using Weapsy.Domain.Menus.Commands;
using Weapsy.Domain.Menus.Events;
using System.Linq;

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

        private Menu(CreateMenuCommand cmd) : base(cmd.Id)
        {
            SiteId = cmd.SiteId;
            Name = cmd.Name;
            Status = MenuStatus.Active;

            AddEvent(new MenuCreatedEvent
            {
                SiteId = SiteId,
                AggregateRootId = Id,
                Name = Name,
                Status = Status
            });
        }

        public static Menu CreateNew(CreateMenuCommand cmd, IValidator<CreateMenuCommand> validator)
        {
            validator.ValidateCommand(cmd);

            return new Menu(cmd);
        }

        public void UpdateName()
        {
            throw new NotImplementedException();
        }

        public void AddMenuItem(AddMenuItemCommand cmd, IValidator<AddMenuItemCommand> validator)
        {
            if (MenuItems.FirstOrDefault(x => x.Id == cmd.MenuItemId) != null)
                throw new Exception("Menu item already added");

            validator.ValidateCommand(cmd);

            var sortOrder = MenuItems.Count(x => x.ParentId == Guid.Empty) + 1;

            MenuItems.Add(new MenuItem(cmd, sortOrder));

            AddEvent(new MenuItemAddedEvent
            {
                SiteId = SiteId,
                AggregateRootId = Id,
                Name = Name,
                MenuItem = MenuItems.Single(x => x.Id == cmd.MenuItemId)
            });
        }

        public void UpdateMenuItem(UpdateMenuItemCommand cmd, IValidator<UpdateMenuItemCommand> validator)
        {
            var menuItem = MenuItems.FirstOrDefault(x => x.Id == cmd.MenuItemId);

            if (menuItem == null || menuItem.Status == MenuItemStatus.Deleted)
                throw new Exception("Menu item not found");

            validator.ValidateCommand(cmd);

            menuItem.Update(cmd);

            AddEvent(new MenuItemUpdatedEvent
            {
                SiteId = SiteId,
                AggregateRootId = Id,
                Name = Name,
                MenuItem = menuItem
            });
        }

        public void ReorderMenuItems(ReorderMenuItemsCommand cmd, IValidator<ReorderMenuItemsCommand> validator)
        {
            validator.ValidateCommand(cmd);

            var reorderedMenuItems = new List<MenuItemsReorderedEvent.MenuItem>();

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

                    reorderedMenuItems.Add(new MenuItemsReorderedEvent.MenuItem
                    {
                        Id = id,
                        ParentId = parentId,
                        SortOrder = sortOrder
                    });
                }
            }

            AddEvent(new MenuItemsReorderedEvent
            {
                SiteId = SiteId,
                AggregateRootId = Id,
                Name = Name,
                MenuItems = reorderedMenuItems
            });
        }

        public void RemoveMenuItem(RemoveMenuItemCommand cmd, IValidator<RemoveMenuItemCommand> validator)
        {
            validator.ValidateCommand(cmd);

            var menuItemToRemove = MenuItems.FirstOrDefault(x => x.Id == cmd.MenuItemId);

            if (menuItemToRemove == null || menuItemToRemove.Status == MenuItemStatus.Deleted)
                throw new Exception("Menu item to remove not found.");

            MarkMenuItemAsDeleted(menuItemToRemove);

            AddEvent(new MenuItemRemovedEvent
            {
                SiteId = SiteId,
                AggregateRootId = Id,
                Name = Name,
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
                MarkMenuItemAsDeleted(subMenuItem);
        }

        public void SetMenuItemPermissions(SetMenuItemPermissionsCommand cmd)
        {
            var menuItem = MenuItems.FirstOrDefault(x => x.Id == cmd.MenuItemId);

            if (menuItem == null)
                throw new Exception("Menu item not found.");

            menuItem.SetPermisisons(cmd.MenuItemPermissions);

            Events.Add(new MenuItemPermissionsSetEvent
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

            Status = MenuStatus.Deleted;

            AddEvent(new MenuDeletedEvent
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