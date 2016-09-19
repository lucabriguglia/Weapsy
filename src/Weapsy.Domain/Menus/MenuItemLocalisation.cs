using System;
using Weapsy.Core.Domain;

namespace Weapsy.Domain.Menus
{
    public class MenuItemLocalisation : ValueObject
    {
        public Guid MenuItemId { get; private set; }
        public Guid LanguageId { get; private set; }
        public string Text { get; private set; }
        public string Title { get; private set; }

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