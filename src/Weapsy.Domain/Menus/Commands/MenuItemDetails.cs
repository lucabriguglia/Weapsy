using System;
using System.Collections.Generic;
using Weapsy.Core.Domain;

namespace Weapsy.Domain.Model.Menus.Commands
{
    public class MenuItemDetails : ICommand
    {
        public Guid SiteId { get; set; }
        public Guid MenuId { get; set; }
        public Guid MenuItemId { get; set; }
        public MenuItemType MenuItemType { get; set; }
        public Guid PageId { get; set; }
        public string Link { get; set; }
        public string Text { get; set; }
        public string Title { get; set; }
        public IList<MenuItemLocalisation> MenuItemLocalisations { get; set; } = new List<MenuItemLocalisation>();

        public class MenuItemLocalisation
        {
            public Guid LanguageId { get; set; }
            public string Text { get; set; }
            public string Title { get; set; }
        }
    }
}
