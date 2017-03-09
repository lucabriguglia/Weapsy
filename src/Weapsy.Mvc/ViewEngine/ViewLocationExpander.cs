using Microsoft.AspNetCore.Mvc.Razor;
using System.Collections.Generic;
using System.Linq;

namespace Weapsy.Mvc.ViewEngine
{
    public class ViewLocationExpander : IViewLocationExpander
    {
        public const string ThemeKey = "Theme";

        public void PopulateValues(ViewLocationExpanderContext context)
        {
            string theme = context.ActionContext.HttpContext.Items[ThemeKey].ToString();
            context.Values[ThemeKey] = theme;
        }

        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            string theme;

            if (context.Values.TryGetValue(ThemeKey, out theme))
            {
                viewLocations = new[] 
                {
                    $"/Themes/{theme}/Views/{{1}}/{{0}}.cshtml",
                    $"/Themes/{theme}/Views/Shared/{{0}}.cshtml",
                    $"/Themes/{theme}/Apps/{{2}}/Views/{{1}}/{{0}}.cshtml",
                    $"/Themes/{theme}/Apps/{{2}}/Views/Shared/{{0}}.cshtml",
                    $"/Themes/{theme}/Areas/{{2}}/Views/{{1}}/{{0}}.cshtml",
                    $"/Themes/{theme}/Areas/{{2}}/Views/Shared/{{0}}.cshtml",
                    "/Apps/{2}/Views/{1}/{0}.cshtml",
                    "/Apps/{2}/Views/Shared/{0}.cshtml"
                }
                .Concat(viewLocations);
            }

            return viewLocations;
        }
    }
}
