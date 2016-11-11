using System.Collections.Generic;

namespace Weapsy.Reporting.Menus
{
    public class MenuViewModel
    {
        public string Name { get; set; }
        public string Culture { get; set; }

        public List<MenuItem> MenuItems { get; set; } = new List<MenuItem>();

        public class MenuItem
        {
            public string Text { get; set; }
            public string Title { get; set; }
            public string Url { get; set; }
            public IEnumerable<string> ViewRoles { get; set; } = new List<string>();

            public List<MenuItem> Children { get; set; } = new List<MenuItem>();
        }
    }
}
