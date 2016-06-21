using System;
using System.Collections.Generic;
using System.Linq;
using Weapsy.Core.Domain;
using Weapsy.Domain.Model.Menus.Commands;

namespace Weapsy.Domain.Model.Menus
{
    public class MenuItem : Entity
    {
        public Guid MenuId { get; private set; }
        public Guid ParentId { get; private set; }
        public int SortOrder { get; private set; }
        public MenuItemType MenuItemType { get; private set; }
        public Guid PageId { get; private set; }
        public string Link { get; private set; }
        public string Text { get; private set; }
        public string Title { get; private set; }
        public MenuItemStatus Status { get; private set; }

        private readonly IList<MenuItemLocalisation> _menuItemLocalisations;
        public IEnumerable<MenuItemLocalisation> MenuItemLocalisations
        {
            get { return _menuItemLocalisations; }
            private set { }
        }

        public MenuItem()
        {
            _menuItemLocalisations = new List<MenuItemLocalisation>();
        }

        public MenuItem(AddMenuItem cmd, int sortOrder) : base(cmd.MenuItemId)
        {
            MenuId = cmd.MenuId;
            SortOrder = sortOrder;
            MenuItemType = cmd.MenuItemType;
            PageId = cmd.PageId;
            Link = cmd.Link;
            Text = cmd.Text;
            Title = cmd.Title;
            Status = MenuItemStatus.Active;

            foreach (var item in cmd.MenuItemLocalisations)
            {
                AddLocalisation(item.LanguageId, item.Text, item.Title);
            }
        }

        public void Update(UpdateMenuItem cmd)
        {
            MenuItemType = cmd.MenuItemType;
            PageId = cmd.PageId;
            Link = cmd.Link;
            Text = cmd.Text;
            Title = cmd.Title;

            _menuItemLocalisations.Clear();

            foreach (var item in cmd.MenuItemLocalisations)
            {
                AddLocalisation(item.LanguageId, item.Text, item.Title);
            }
        }

        private void AddLocalisation(Guid languageId, string text, string title)
        {
            if (MenuItemLocalisations.FirstOrDefault(x => x.LanguageId == languageId) != null)
                throw new Exception("Language already added.");

            _menuItemLocalisations.Add(new MenuItemLocalisation(Id, languageId, text, title));
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