using System;
using Weapsy.Framework.Domain;

namespace Weapsy.Domain.Menus
{
    public class MenuItemLocalisation : ValueObject
    {
        public Guid MenuItemId { get; set; }
        public Guid LanguageId { get; set; }
        public string Text { get; set; }
        public string Title { get; set; }

        public MenuItemLocalisation(){}

        public MenuItemLocalisation(Guid menuItemId, Guid languageId, string text, string title)
        {
            MenuItemId = menuItemId;
            LanguageId = languageId;
            Text = text;
            Title = title;
        }
    }
}